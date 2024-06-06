using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {
        Socket server;
        NetworkStream ns;
        byte[] data = new byte[1024];
        private readonly byte[] key = Encoding.UTF8.GetBytes("1234567890123456");
        private readonly byte[] iv = Encoding.UTF8.GetBytes("1234567890123456");  

        public Form1()
        {
            InitializeComponent();
        }

        private async void Connect_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress host = IPAddress.Parse("127.0.0.1");
                IPEndPoint hostep = new IPEndPoint(host, 9050);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                await Task.Run(() => server.Connect(hostep));
                ContentBox.Text += "Client: Socket connected to " + server.RemoteEndPoint?.ToString() + "\n";
                Connect.Enabled = false;
                Send.Enabled = true;
                Close.Enabled = true;
                MessageBox.Enabled = true;

                ns = new NetworkStream(server);

                int recv = ns.Read(data, 0, data.Length);
                ContentBox.Text += "Server: " + Encoding.ASCII.GetString(data, 0, recv) + "\n";

                while (true)
                {
                    data = new byte[1024];
                    recv = await ns.ReadAsync(data, 0, data.Length);
                    if (recv == 0)
                        break;

                    string response = Encoding.ASCII.GetString(data, 0, recv);

                    if (response.StartsWith("GET"))
                    {
                        string filePath = response.Substring(4).Trim();
                        if (File.Exists(filePath))
                        {
                            await SendFile(filePath);
                        }
                        else
                        {
                            data = Encoding.ASCII.GetBytes("ERROR: File not found");
                            await ns.WriteAsync(data, 0, data.Length);
                        }
                    }



                    if (response.StartsWith("FILE:"))
                    {
                        string[] parts = response.Split(':');
                        string fileName = parts[1];
                        int fileLength = int.Parse(parts[2]);

                        byte[] fileData = new byte[fileLength];
                        int totalRead = 0;

                        while (totalRead < fileLength)
                        {
                            int bytesRead = await ns.ReadAsync(fileData, totalRead, fileData.Length - totalRead);
                            totalRead += bytesRead;
                        }

                        string filePath = Path.Combine("ReceivedFiles", fileName);
                        Directory.CreateDirectory("ReceivedFiles");
                        await Task.Run(() => File.WriteAllBytes(filePath, fileData));
                        ContentBox.Text += $"File received and saved to {filePath}\n";

                        if (fileName.EndsWith(".enc"))
                        {
                            string decryptedFilePath = DecryptFile(filePath);
                            ContentBox.Text += $"File decrypted and saved to {decryptedFilePath}\n";
                            DisplayImage(decryptedFilePath); 
                        }
                        else
                        {
                            DisplayImage(filePath); 
                        }
                    }
                    else
                    {
                        ContentBox.Text += "Server: " + response + "\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ContentBox.Text += $"Client Error: {ex.Message}\n";
            }
        }
        private async Task SendFile(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            byte[] fileData = File.ReadAllBytes(filePath);
            byte[] header = Encoding.ASCII.GetBytes("FILE:" + fileName + ":" + fileData.Length.ToString() + "\n");
            await ns.WriteAsync(header, 0, header.Length);
            await ns.WriteAsync(fileData, 0, fileData.Length);
        }

        private async void Send_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] dt = Encoding.ASCII.GetBytes(MessageBox.Text);
                await ns.WriteAsync(dt, 0, dt.Length);
                ContentBox.Text += "Client: " + MessageBox.Text + "\n";
                MessageBox.Text = string.Empty;
            }
            catch (Exception ex)
            {
                ContentBox.Text += $"Send Error: {ex.Message}\n";
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            try
            {
                ns.Close();
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                Connect.Enabled = true;
                Send.Enabled = false;
                Close.Enabled = false;
                MessageBox.Enabled = false;
            }
            catch (Exception ex)
            {
                ContentBox.Text += $"Close Error: {ex.Message}\n";
            }
        }

        private string DecryptFile(string filePath)
        {
            string decryptedFilePath = filePath.Replace(".enc", string.Empty);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (FileStream fsEncrypted = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (FileStream fsDecrypted = new FileStream(decryptedFilePath, FileMode.Create, FileAccess.Write))
                using (CryptoStream csDecrypt = new CryptoStream(fsDecrypted, decryptor, CryptoStreamMode.Write))
                {
                    fsEncrypted.CopyTo(csDecrypt);
                }
            }

            return decryptedFilePath;
        }

        private void DisplayImage(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (var image = Image.FromFile(filePath))
                    {
                        pictureBox2.Image = new Bitmap(image);
                    }
                }
                else
                {
                    ContentBox.Text += "Error: File not found for display\n";
                }
            }
            catch (Exception ex)
            {
                ContentBox.Text += $"Display Image Error: {ex.Message}\n";
            }
        }
    }
}
