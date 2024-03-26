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
        int chatPort = 27500;
        int recPort = 27000;
        string chatSendStr = "127.0.0.1";
        int TcpPort = 30000;

        int imageNumber = 1;

        UdpClient udpClnt = new UdpClient();
        Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private TcpClient? connectedClient;
        // For test [---
        private string lastReceivedMessage = string.Empty; // Field to store the last sent message
        private IUdpClientWrapper _udpClientWrapper;
        private IMessageBoxWrapper _messageBoxWrapper;
        private ITcpClientWrapper _tcpClientWrapper;

        public Form1(ITcpClientWrapper tcpClientWrapper) : this() // Call the default constructor first
        {
            _tcpClientWrapper = tcpClientWrapper;
        }
        // ---]

        public Form1()
        {
            InitializeComponent();
            textBox1.KeyPress += textBox1_KeyPress;

            InitializeConnection();

            StartReceivingMessages();
            StartReceivingImages();
        }

        private async void InitializeConnection()
        {
            try
            {
                if (connectedClient == null)
                {
                    connectedClient = new TcpClient(); // Instantiate a new TcpClient if not already initialized
                }

                IPAddress ipAddress = IPAddress.Parse(chatSendStr);
                var ipEndPoint = new IPEndPoint(ipAddress, TcpPort);

                if (!connectedClient.Connected) // Check if the client is already connected
                {
                    await connectedClient.ConnectAsync(ipEndPoint); // Connect to the server asynchronously
                }

                // Connection successful, perform additional initialization or operations
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting: " + ex.Message);
                // Handle the error appropriately
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Close the UDP socket if it's open
            if (udpClnt != null)
            {
                udpClnt.Close();
            }
            if (connectedClient != null)
            {
                connectedClient.Close();
            }
        }

        public void StartReceivingMessages()
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

        public void SendMessage()
        {
            string newText = textBox1.Text;

            // Checking to see is string is empty
            if (String.IsNullOrEmpty(newText))
            {
                MessageBox.Show("Cannot send empty messages.", "Cannot Send");
                return; // Return immediately after displaying the error message
            }

            try
            {
                UdpClient udpClient = new UdpClient();
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(chatSendStr), chatPort);
                byte[] data = Encoding.ASCII.GetBytes(newText);
                udpClient.Send(data, data.Length, ipEndPoint);
                lastReceivedMessage = newText; // Update the last received message whenever a new one is displayed
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

        public void DisplayReceivedMessage(string message)
        {
            lastReceivedMessage = message; // Update the last received message whenever a new one is displayed

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

        // Attachment functionality -- TODO: implement click functionality to the attach button
        public async Task ReceiveImagesFromServer()
        {
            while (true)
            {
                try
                {
                    if (connectedClient == null || !connectedClient.Connected)
                    {
                        MessageBox.Show("TCP client is not initialized or not connected.");
                        return; // Or you might want to attempt to reconnect here
                    }
                    // Read the byte array length prefix
                    byte[] lengthPrefix = new byte[4]; // Assuming int32 length prefix
                    await connectedClient.GetStream().ReadAsync(lengthPrefix, 0, 4);
                    int imageDataLength = BitConverter.ToInt32(lengthPrefix, 0);

                    // Read the byte array containing the image data
                    byte[] imageData = new byte[imageDataLength];
                    int totalBytesRead = 0;
                    while (totalBytesRead < imageDataLength)
                    {
                        int bytesRead = await connectedClient.GetStream().ReadAsync(imageData, totalBytesRead, imageDataLength - totalBytesRead);
                        if (bytesRead == 0)
                        {
                            throw new IOException("End of stream reached before image data could be fully received.");
                        }
                        totalBytesRead += bytesRead;
                    }

                    // Deserialize the byte array back into an image
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        System.Drawing.Image receivedImage = System.Drawing.Image.FromStream(ms);
                        // Use the received image as needed
                        SaveImageToFile(receivedImage);
                        ReceivedImageLink();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error receiving image: " + ex.Message);
                }
            }
        }


        // Call the asynchronous method to start receiving images from the server
        private void StartReceivingImages()
        {
            Task.Run(() => ReceiveImagesFromServer());
        }

        // Label linking to the image
        public void ReceivedImageLink()
        {
            // Invokes change to chat panel
            if (chatContainerPanel.InvokeRequired)
            {
                chatContainerPanel.Invoke(new MethodInvoker(() => ReceivedImageLink()));
                return;
            }

            ImageLinkLabel imageLink = new ImageLinkLabel();
            imageLink.setTextLink($"image{imageNumber}.jpeg");
            imageNumber++;
            int newY = (chatContainerPanel.Controls.Count > 0) ? chatContainerPanel.Controls[chatContainerPanel.Controls.Count - 1].Bottom + 10 : 0;
            imageLink.Location = new Point(0, newY);
            chatContainerPanel.Controls.Add(imageLink);

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

        // --- For testing --- //

        public string GetLastReceivedMessage()
        {
            return lastReceivedMessage;
        }

        public void SetMessageBoxWrapper(IMessageBoxWrapper messageBoxWrapper)
        {
            _messageBoxWrapper = messageBoxWrapper;
        }

        public void SetTextBoxText(string text)
        {
            textBox1.Text = text;
        }

        public void SetUdpClientWrapper(IUdpClientWrapper udpClientWrapper)
        {
            _udpClientWrapper = udpClientWrapper;
        }

        public void SendUdpMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message cannot be null or empty", nameof(message));
            }

            if (_udpClientWrapper == null)
            {
                throw new InvalidOperationException("UDP client wrapper is not set.");
            }

            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            // For the purpose of this example, we assume the destination IP and port are predefined or obtained from elsewhere.
            string destinationIp = "127.0.0.1";
            int destinationPort = 27500;

            // Use the wrapper to send the message
            _udpClientWrapper.Send(messageBytes, messageBytes.Length, destinationIp, destinationPort);
        }
        public class TcpClientWrapper : ITcpClientWrapper
        {
            private TcpClient _client = new TcpClient();

            public bool Connected => _client.Connected;

            public Task ConnectAsync(IPAddress address, int port) => _client.ConnectAsync(address, port);

            public INetworkStream GetStream() => new NetworkStreamWrapper(_client.GetStream()); // Wrap and return INetworkStream

            public void Close() => _client.Close();
        }


        public class NetworkStreamWrapper : INetworkStream
        {
            private readonly NetworkStream _networkStream;

            public NetworkStreamWrapper(NetworkStream networkStream)
            {
                _networkStream = networkStream;
            }

            public async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                return await _networkStream.ReadAsync(buffer, offset, count, cancellationToken);
            }

            public void Write(byte[] buffer, int offset, int count)
            {
                _networkStream.Write(buffer, offset, count);
            }

        }


    }

    public interface IMessageBoxWrapper
    {
        void Show(string message, string caption);
    }

    public interface IUdpClientWrapper
    {
        void DisplaySentMessage(string testMessage);
        Task<UdpReceiveResult> ReceiveAsync(); // Assuming UdpReceiveResult is a stand-in for your data packet
        void Send(byte[] data, int length, string ip, int port);
    }

    public interface ITcpClientWrapper
    {
        bool Connected { get; }
        Task ConnectAsync(IPAddress address, int port);
        INetworkStream GetStream(); // Return type changed to INetworkStream
        void Close();
    }

    public interface INetworkStream
    {
        Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
        void Write(byte[] buffer, int offset, int count);
    }

}
