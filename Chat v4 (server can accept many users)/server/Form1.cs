using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace server
{
    public partial class Form1 : Form
    {
        private Socket server;
        private byte[] data = new byte[1024];
        private readonly byte[] key = Encoding.UTF8.GetBytes("1234567890123456"); // Example key
        private readonly byte[] iv = Encoding.UTF8.GetBytes("1234567890123456");  // Example IV
        private ConcurrentDictionary<int, NetworkStream> clientStreams = new ConcurrentDictionary<int, NetworkStream>();
        private int clientIdCounter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Listen_Click(object sender, EventArgs e)
        {
            try
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(ipep);
                server.Listen(10);

                ContentBox.Text += "Server: I'm Listening on 127.0.0.1:9050\n";

                while (true)
                {
                    Socket client = await Task.Run(() => server.Accept());
                    int clientId = clientIdCounter++;
                    Task.Run(() => HandleClient(client, clientId)); // Handle each client in a separate task
                }
            }
            catch (Exception ex)
            {
                ContentBox.Text += $"Server Error: {ex.Message}\n";
            }
        }

        private async Task HandleClient(Socket client, int clientId)
        {
            NetworkStream ns = new NetworkStream(client);
            clientStreams[clientId] = ns;
            try
            {
                ContentBox.Invoke(new Action(() => ContentBox.Text += $"Client {clientId} connected.\n"));

                string welcome = "Welcome to the test server!";
                data = Encoding.ASCII.GetBytes(welcome + "\n");
                await ns.WriteAsync(data, 0, data.Length);

                while (true)
                {
                    data = new byte[1024];
                    int recv = await ns.ReadAsync(data, 0, data.Length);
                    if (recv == 0)
                        break;

                    string request = Encoding.ASCII.GetString(data, 0, recv).Trim();
                    ContentBox.Invoke(new Action(() => ContentBox.Text += $"Client {clientId}: {request}\n"));

                    if (request.StartsWith("DIR"))
                    {
                        string directoryPath = request.Substring(4).Trim();
                        if (Directory.Exists(directoryPath))
                        {
                            var files = Directory.GetFiles(directoryPath);
                            var directories = Directory.GetDirectories(directoryPath);
                            StringBuilder response = new StringBuilder("FILES\n");
                            foreach (var dir in directories)
                            {
                                response.AppendLine("DIR:" + dir);
                            }
                            foreach (var file in files)
                            {
                                response.AppendLine("FILE:" + file);
                            }
                            data = Encoding.ASCII.GetBytes(response.ToString() + "\n");
                            await ns.WriteAsync(data, 0, data.Length);
                        }
                        else
                        {
                            data = Encoding.ASCII.GetBytes("ERROR: Directory not found\n");
                            await ns.WriteAsync(data, 0, data.Length);
                        }
                    }
                    else if (request.StartsWith("GET"))
                    {
                        string filePath = request.Substring(4).Trim();
                        if (File.Exists(filePath))
                        {
                            await SendFile(filePath, ns);
                        }
                        else
                        {
                            data = Encoding.ASCII.GetBytes("ERROR: File not found\n");
                            await ns.WriteAsync(data, 0, data.Length);
                        }
                    }
                    else if (request.StartsWith("Encrypt"))
                    {
                        string filePath = request.Substring(8).Trim();
                        if (File.Exists(filePath))
                        {
                            string encryptedFilePath = EncryptFile(filePath);
                            await SendFile(encryptedFilePath, ns);
                        }
                        else
                        {
                            data = Encoding.ASCII.GetBytes("ERROR: File not found\n");
                            await ns.WriteAsync(data, 0, data.Length);
                        }
                    }
                    else if (request.StartsWith("Decrypt"))
                    {
                        string filePath = request.Substring(8).Trim();
                        if (File.Exists(filePath))
                        {
                            string decryptedFilePath = DecryptFile(filePath);
                            await SendFile(decryptedFilePath, ns);
                        }
                        else
                        {
                            data = Encoding.ASCII.GetBytes("ERROR: File not found\n");
                            await ns.WriteAsync(data, 0, data.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ContentBox.Invoke(new Action(() => ContentBox.Text += $"Handle Client {clientId} Error: {ex.Message}\n"));
            }
            finally
            {
                ns.Close();
                client.Close();
                clientStreams.TryRemove(clientId, out _);
                ContentBox.Invoke(new Action(() => ContentBox.Text += $"Client {clientId} disconnected.\n"));
            }
        }

        private async Task SendFile(string filePath, NetworkStream ns)
        {
            string fileName = Path.GetFileName(filePath);
            string serverDownloadPath = Path.Combine("ServerDownloads", fileName);
            Directory.CreateDirectory("ServerDownloads");
            File.Copy(filePath, serverDownloadPath, true);

            byte[] fileData = File.ReadAllBytes(filePath);
            byte[] header = Encoding.ASCII.GetBytes("FILE:" + fileName + ":" + fileData.Length.ToString() + "\n");
            await ns.WriteAsync(header, 0, header.Length);
            await ns.WriteAsync(fileData, 0, fileData.Length);
        }

        private string EncryptFile(string filePath)
        {
            string encryptedFilePath = filePath + ".enc";

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (FileStream fsInput = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (FileStream fsEncrypted = new FileStream(encryptedFilePath, FileMode.Create, FileAccess.Write))
                using (CryptoStream csEncrypt = new CryptoStream(fsEncrypted, encryptor, CryptoStreamMode.Write))
                {
                    fsInput.CopyTo(csEncrypt);
                }
            }

            return encryptedFilePath;
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

        private void Send_Click(object sender, EventArgs e)
        {
            try
            {
                string message = MessageBox.Text;
                byte[] dt = Encoding.ASCII.GetBytes(message);

                foreach (var kvp in clientStreams)
                {
                    NetworkStream ns = kvp.Value;
                    ns.Write(dt, 0, dt.Length);
                }

                ContentBox.Text += "Server: " + MessageBox.Text + "\n";
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
                foreach (var kvp in clientStreams)
                {
                    kvp.Value.Close();
                }

                server.Close();
                Listen.Enabled = true;
                Close.Enabled = false;
                Send.Enabled = false;
                MessageBox.Enabled = false;
            }
            catch (Exception ex)
            {
                ContentBox.Text += $"Close Error: {ex.Message}\n";
            }
        }
    }
}
