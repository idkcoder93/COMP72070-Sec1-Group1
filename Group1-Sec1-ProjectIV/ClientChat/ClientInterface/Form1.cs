using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Policy;
using System.Net.Http;


namespace ClientInterface
{
    public partial class Form1 : Form
    {

        int chatPort = 27000;
        int recPort = 27500;
        string chatSendStr = "127.0.0.1";
        int TcpPort = 30000;

        UdpClient udpClnt = new UdpClient();
        Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private TcpListener listener;
        private TcpClient? connectedClient;

        public Form1()
        {
            InitializeComponent();
            textBox1.KeyPress += textBox1_KeyPress;

            StartReceivingMessages();

            // Initialize TcpListener
            IPAddress ipAddress = IPAddress.Parse(chatSendStr);
            var ipEndPoint = new IPEndPoint(ipAddress, TcpPort);
            listener = new TcpListener(ipEndPoint);
            listener.Start();
            // Start accepting TCP clients asynchronously
            _ = AcceptTcpClient();
        }

        private async Task AcceptTcpClient()
        {
            try
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                connectedClient = client;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error accepting TCP client: " + ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Close the UDP socket if it's open
            if (udpClnt != null)
            {
                udpClnt.Close();
            }
        }

        private void StartReceivingMessages()
        {
            // UI thread for dynamic elements
            Task t = new Task(() => { ReceiveMessages(); });
            t.Start();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enter-key pressed will send message
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                SendMessage();
            }
        }

        private void SendMessage()
        {
            string newText = textBox1.Text;

            // Checking to see is string is empty
            if (String.IsNullOrEmpty(newText))
            {
                MessageBox.Show("Cannot send empty messages.", "Cannot Send");
                return;
            }

            try
            {
                UdpClient udpClient = new UdpClient();
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(chatSendStr), chatPort);
                byte[] data = Encoding.ASCII.GetBytes(newText);
                udpClient.Send(data, data.Length, ipEndPoint);
                DisplaySentMessage(newText);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Message transfer error: " + ex.Message);
            }

            textBox1.Clear();
        }

        private void DisplaySentMessage(string message)
        {
            MeChatBubble meChatBubble1 = new MeChatBubble();
            MeChatBubble.SetBubbleText(meChatBubble1, message);
            int newY = (chatContainerPanel.Controls.Count > 0) ? chatContainerPanel.Controls[chatContainerPanel.Controls.Count - 1].Bottom + 10 : 0;
            meChatBubble1.Location = new Point(0, newY);
            chatContainerPanel.Controls.Add(meChatBubble1);
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[512];

            UdpClient udpClnt = new UdpClient();

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, recPort); // Bind to any IP address with that port

            udpClnt.ExclusiveAddressUse = false;
            udpClnt.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClnt.Client.Bind(ipEndPoint);


            while (true)
            {
                try
                {
                    int bytesReady = udpClnt.Available;

                    while (bytesReady > 0)
                    {
                        try
                        {
                            var buff = new byte[bytesReady];
                            buff = udpClnt.Receive(ref ipEndPoint); // Referencing end-point of data communication
                            string strData = Encoding.ASCII.GetString(buff, 0, buff.Length);
                            DisplayReceivedMessage(strData);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                        bytesReady = 0;
                    }

                    Thread.Sleep(1000); // Sleep for 1 second
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during receiving: " + ex.Message);
                }
            }
        }

        private void DisplayReceivedMessage(string message)
        {
            // Invokes change to chat panel
            if (chatContainerPanel.InvokeRequired)
            {
                chatContainerPanel.Invoke(new MethodInvoker(() => DisplayReceivedMessage(message)));
                return;
            }

            YouChatBubble youChatBubble = new YouChatBubble();
            YouChatBubble.SetBubbleText(youChatBubble, message);
            int newY = (chatContainerPanel.Controls.Count > 0) ? chatContainerPanel.Controls[chatContainerPanel.Controls.Count - 1].Bottom + 10 : 0;
            youChatBubble.Location = new Point(0, newY);
            chatContainerPanel.Controls.Add(youChatBubble);
        }

        // Attachment functionality
        private async void OnClickAttachButton(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|All files (*.*)|*.*"; // Limit to JPEG files
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;

                try
                {
                    // Check if the selected file is a JPEG image
                    if (Path.GetExtension(file).Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        Path.GetExtension(file).Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
                    {
                        // Server side code
                        Bitmap tImage = new Bitmap(file);
                        byte[] imageBytes;

                        using (MemoryStream ms = new MemoryStream())
                        {
                            tImage.Save(ms, tImage.RawFormat); // Save the Bitmap to the MemoryStream
                            imageBytes = ms.ToArray(); // Convert MemoryStream to byte array
                        }

                        // If a client is not already connected, accept a new one
                        if (connectedClient == null)
                        {
                            return;
                        }
                        if (connectedClient != null)
                        {
                            byte[] lengthPrefix = BitConverter.GetBytes(imageBytes.Length);
                            await connectedClient.GetStream().WriteAsync(lengthPrefix, 0, lengthPrefix.Length);

                            // Send the byte array containing the image data
                            await connectedClient.GetStream().WriteAsync(imageBytes, 0, imageBytes.Length);
                        }
                        else
                        {
                            MessageBox.Show("Error: Unable to establish connection with the client.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a JPEG image file.");
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private void SaveImageToFile(System.Drawing.Image img)
        {
            // Get the directory where the executable file is located
            string directory = System.Windows.Forms.Application.StartupPath;

            // Construct the relative path to save the image file
            string relativePath = Path.Combine(directory, "image.jpeg");

            // Save the image to the specified file path
            img.Save(relativePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

    }
}

/* This is just in case -- backup

OpenFileDialog openFileDialog1 = new OpenFileDialog();
openFileDialog1.Filter = "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|All files (*.*)|*.*"; // Limit to JPEG files
DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.

if (result == DialogResult.OK)
{
    string file = openFileDialog1.FileName;

    try
    {
        // Check if the selected file is a JPEG image
        if (Path.GetExtension(file).Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
            Path.GetExtension(file).Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
        {
            // Server side code
            Bitmap tImage = new Bitmap(file);
            byte[] imageBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                tImage.Save(ms, tImage.RawFormat); // Save the Bitmap to the MemoryStream
                imageBytes = ms.ToArray(); // Convert MemoryStream to byte array
            }

            // If a client is not already connected, accept a new one
            if (connectedClient == null)
            {
                //connectedClient = await AcceptTcpClient();
            }
            if (connectedClient != null)
            {
                // Send the image data to the client
                await connectedClient.GetStream().WriteAsync(imageBytes, 0, imageBytes.Length);
                connectedClient.GetStream().Close();
            }
            else
            {
                MessageBox.Show("Error: Unable to establish connection with the client.");
            }
        }
        else
        {
            MessageBox.Show("Please select a JPEG image file.");
        }
    }
    catch (IOException ex)
    {
        MessageBox.Show("Error: " + ex.Message);
    }
} */