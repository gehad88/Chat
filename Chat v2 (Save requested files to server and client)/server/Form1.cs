using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class Form1 : Form
    {
        Socket server;
        Socket client;
        NetworkStream ns;
        byte[] data = new byte[1024];

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

                client = await Task.Run(() => server.Accept());
                MessageBox.Enabled = true;
                Close.Enabled = true;
                Send.Enabled = true;
                Listen.Enabled = false;

                ns = new NetworkStream(client);
                ContentBox.Text += "Client connected.\n";

                string welcome = "Welcome to the test server!";
                data = Encoding.ASCII.GetBytes(welcome);
                ns.Write(data, 0, data.Length);

                while (true)
                {
                    data = new byte[1024];
                    int recv = await ns.ReadAsync(data, 0, data.Length);
                    if (recv == 0)
                        break;

                    string request = Encoding.ASCII.GetString(data, 0, recv).Trim();
                    ContentBox.Text += $"Client: {request}\n";

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
                            data = Encoding.ASCII.GetBytes(response.ToString());
                            await ns.WriteAsync(data, 0, data.Length);
                        }
                        else
                        {
                            data = Encoding.ASCII.GetBytes("ERROR: Directory not found");
                            await ns.WriteAsync(data, 0, data.Length);
                        }
                    }
                    else if (request.StartsWith("GET"))
                    {
                        string filePath = request.Substring(4).Trim();
                        if (File.Exists(filePath))
                        {
                            string fileName = Path.GetFileName(filePath);
                            string serverDownloadPath = Path.Combine("ServerDownloads", fileName);
                            Directory.CreateDirectory("ServerDownloads");
                            File.Copy(filePath, serverDownloadPath, true);

                            if (filePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                filePath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                            {
                                byte[] fileData = File.ReadAllBytes(filePath);
                                byte[] header = Encoding.ASCII.GetBytes("IMG:" + fileData.Length.ToString() + "\n");
                                await ns.WriteAsync(header, 0, header.Length);
                                await ns.WriteAsync(fileData, 0, fileData.Length);
                            }
                            else
                            {
                                string fileContent = File.ReadAllText(filePath);
                                data = Encoding.ASCII.GetBytes(fileContent);
                                await ns.WriteAsync(data, 0, data.Length);
                            }
                        }
                        else
                        {
                            data = Encoding.ASCII.GetBytes("ERROR: File not found");
                            await ns.WriteAsync(data, 0, data.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ContentBox.Text += $"Server Error: {ex.Message}\n";
            }
        }

        private void Send_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] dt = Encoding.ASCII.GetBytes(MessageBox.Text);
                ns.Write(dt, 0, dt.Length);
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
                ns.Close();
                client.Close();
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
 