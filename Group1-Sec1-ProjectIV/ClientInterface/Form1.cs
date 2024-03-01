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


namespace ClientInterface
{
    public partial class Form1 : Form
{
    //bool chatConnected = false;
    int chatPort = 27000;
    int recPort = 27500;
    //bool receiveData = false;
    string chatSendStr = "127.0.0.1";

    UdpClient udpClnt = new UdpClient();
    Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    IPEndPoint? endPnt;

        public Form1()
    {
        InitializeComponent();
        textBox1.KeyPress += textBox1_KeyPress;

            StartReceivingMessages();
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
        Task t = new Task(() => { ReceiveMessages(); });
        t.Start();
    }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            SendMessage();
        }
    }

    private void SendMessage()
    {
        string newText = textBox1.Text;

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

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, recPort); // Bind to specific IP address

            udpClnt.ExclusiveAddressUse = false;
            udpClnt.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClnt.Client.Bind(ipEndPoint);

            //var from = new IPEndPoint(0, 0);

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
                            buff = udpClnt.Receive(ref ipEndPoint);
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


}
}
