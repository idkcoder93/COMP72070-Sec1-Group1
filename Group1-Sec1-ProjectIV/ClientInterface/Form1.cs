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


namespace ClientInterface
{
    public partial class Form1 : Form
    {

        int chatPort = 27000;
        int recPort = 27500;
        string chatSendStr = "127.0.0.1";

        UdpClient udpClnt = new UdpClient();
        Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint? endPnt;

        public Form1()
        {
            InitializeComponent();
            textBox1.KeyPress += textBox1_KeyPress;

            StartReceivingMessages();
            TCPImageReceive();
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
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), chatPort);
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
        private void OnClickAttachButton(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|All files (*.*)|*.*"; // Limit to JPEG files
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    // Check if the selected file is a JPEG image
                    if (Path.GetExtension(file).Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        Path.GetExtension(file).Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
                    {
                        byte[] imageData = File.ReadAllBytes(file);
                        TCPImageSend(imageData);
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


        private void TCPImageSend(byte[] image)
        {
            int bytesInPackets = 1024;
            int size = image.Length;
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 30000);

            try
            {
                listener.Start();
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                while (size > 0)
                {
                    int bytesToSend = Math.Min(bytesInPackets, size); // Compares which one is smaller
                    stream.Write(image, image.Length - size, bytesToSend); // Send a portion of the image
                    size -= bytesToSend;
                }

                stream.Close();
                client.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            finally
            {
                listener.Stop();
            }
        }

        private void TCPImageReceive()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 35000);

            try
            {
                listener.Start();
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[1024]; // Incoming packets
                int totalBytesReceived = 0;
                using (MemoryStream imageStream = new MemoryStream())
                {
                    int bytesRead;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        imageStream.Write(buffer, 0, bytesRead);
                        totalBytesReceived += bytesRead;
                    }

                    // After receiving all image data, call the ImageDeserializer function
                    ImageDeserializer(imageStream.ToArray());
                }

                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            finally
            {
                listener.Stop();
            }
        }


        private void ImageDeserializer(byte[] data)
        {
            using (MemoryStream memstr = new MemoryStream(data))
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(memstr);
                SaveImageToFile(img);
            }
        }

        private void SaveImageToFile(System.Drawing.Image img)
        {
            // Specify the file path where you want to save the image
            string filePath = @"\\Mac\\Home\\Desktop\\ProjectIV_Group1\\Group1-Sec1-ProjectIV\\ClientInterface\\Resources\\image.jpeg";

            // Save the image to the specified file path
            img.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

    }
}
