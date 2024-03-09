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
        //IPEndPoint? endPnt;

        public Form1()
        {
            InitializeComponent();
            textBox1.KeyPress += textBox1_KeyPress;

            StartReceivingMessages();
            StartReceivingImages();
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
                lastSentMessage = newText; 
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

        // Attachment functionality -- TODO: implement click functionality to the attach button
        private async Task ReceiveImagesFromServer()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(chatSendStr);
                var ipEndPoint = new IPEndPoint(ipAddress, TcpPort);
                using TcpClient client = new();
                await client.ConnectAsync(ipEndPoint);

                while (true) // Continuously listen for incoming images
                {
                    NetworkStream nNetStream = client.GetStream();
                    System.Drawing.Image returnImage = System.Drawing.Image.FromStream(nNetStream);
                    SaveImageToFile(returnImage);
                    ReceivedImageLink();

                    // Add some delay to prevent busy waiting (optional)
                    await Task.Delay(1000);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
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

        private IUdpClientWrapper _udpClientWrapper;
        private IMessageBoxWrapper _messageBoxWrapper;

        private string lastSentMessage; // Field to store the last sent message
        public string GetLastSentMessage()
        {
            return lastSentMessage;
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
    }

    public interface IMessageBoxWrapper
    {
        void Show(string message, string caption);
    }

    public interface IUdpClientWrapper
    {
        void DisplaySentMessage(string testMessage);
        void Send(byte[] data, int length, string ip, int port);
    }
}
