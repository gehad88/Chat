using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {
        Socket server;
        NetworkStream ns;
        byte[] data = new byte[1024];

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
                    if (response.StartsWith("IMG:"))
                    {
                        int imageSize = int.Parse(response.Substring(4).Trim());
                        byte[] imageData = new byte[imageSize];
                        int bytesRead = 0;
                        while (bytesRead < imageSize)
                        {
                            int chunkSize = await ns.ReadAsync(imageData, bytesRead, imageSize - bytesRead);
                            bytesRead += chunkSize;
                        }
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            pictureBox2.Image = Image.FromStream(ms);
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
                ContentBox.Text += $"Connect Error: {ex.Message}\n";
            }
        }

        private void Send_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] dt = Encoding.ASCII.GetBytes(MessageBox.Text);
                ns.Write(dt, 0, dt.Length);
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
                server.Close();
                Connect.Enabled = true;
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
