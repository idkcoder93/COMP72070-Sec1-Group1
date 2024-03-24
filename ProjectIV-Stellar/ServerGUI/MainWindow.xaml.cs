using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace ServerGUI
{
    public partial class MainWindow : Window
    {
        // set up the send and receive ports 
        private UdpClient server27000;
        private UdpClient server28000;
        private Thread listenThread27000;
        private Thread listenThread28000;
        private const int port27000 = 27000;
        private const int port27500 = 27500;
        private const int port28000 = 28000;
        private const int port28500 = 28500;
        private int messagesReceived27000 = 0;
        private int messagesReceived28000 = 0;

        public MainWindow()
        {
            InitializeComponent();
            StartServers();
        }

        // function to start the servers and start listening 
        private void StartServers()
        {
            try
            {
                server27000 = new UdpClient(port27000);
                server28000 = new UdpClient(port28000);

                listenThread27000 = new Thread(ListenForClients27000);
                listenThread28000 = new Thread(ListenForClients28000);

                listenThread27000.Start();
                listenThread28000.Start();

                // update UI element indicating that the servers have started
                Dispatcher.Invoke(() =>
                {
                    serverStatusLabel.Content = "Servers started";
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting servers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // listen for connection from client1
        private void ListenForClients27000()
        {
            try
            {
                while (true)
                {
                    IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] clientData = server27000.Receive(ref clientEndPoint);

                    Thread clientThread = new Thread(() => HandleClient27000(clientData, clientEndPoint));
                    clientThread.Start();
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Error receiving data on port 27000: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // listen for connection from client2
        private void ListenForClients28000()
        {
            try
            {
                while (true)
                {
                    IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] clientData = server28000.Receive(ref clientEndPoint);

                    Thread clientThread = new Thread(() => HandleClient28000(clientData, clientEndPoint));
                    clientThread.Start();
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Error receiving data on port 28000: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // receive images and messages 
        // saves data to file 
        // updates server GUI elements 
        private void HandleClient27000(byte[] data, IPEndPoint clientEndPoint)
        {
            try
            {
                string dataReceived = Encoding.ASCII.GetString(data);

                if (dataReceived.StartsWith("[Image]"))
                {
                    byte[] imageData = Convert.FromBase64String(dataReceived.Substring(7)); // Remove [Image] prefix
                    File.WriteAllBytes("ReceivedImage27000.jpg", imageData);
                    Dispatcher.Invoke(() =>
                    {
                        latestMessage.Content = "Received image from Client 1 (Port 27000)";
                    });
                }
                else
                {
                    // Append the received data to a text file
                    SaveDataToFile(dataReceived, "ReceivedData27000.txt"); // save the data to file 

                    Dispatcher.Invoke(() =>
                    {
                        serverStatusLabel.Content = "Client Connected (Port 27000)";
                        numUsersLabel.Content = "True";

                        latestMessageReceivedTime.Content = DateTime.Now.ToString(); // update received message time

                        messagesReceived27000++;
                        numReceivedMessages.Content = messagesReceived27000 + messagesReceived28000;

                        latestMessage.Content = dataReceived; // display latest message 

                        if (dataReceived == "[Disconnect]")
                        {
                            serverStatusLabel.Content = "Client Disconnected (Port 27000)";
                        }
                    });

                    // Forward the received data to port 28500
                    server28000.Send(data, data.Length, new IPEndPoint(clientEndPoint.Address, port28500)); // send this info to client 2 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling client connection on port 27000: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void HandleClient28000(byte[] data, IPEndPoint clientEndPoint)
        {
            try
            {
                string dataReceived = Encoding.ASCII.GetString(data);

                if (dataReceived.StartsWith("[Image]"))
                {
                    byte[] imageData = Convert.FromBase64String(dataReceived.Substring(7)); // remove [Image] prefix
                    File.WriteAllBytes("ReceivedImage28000.jpg", imageData);
                    Dispatcher.Invoke(() =>
                    {
                        latestMessage.Content = "Received image from Client 2 (Port 28000)";
                    });
                }
                else
                {
                    // append the received data to a text file
                    SaveDataToFile(dataReceived, "ReceivedData28000.txt");

                    Dispatcher.Invoke(() =>
                    {
                        serverStatusLabel.Content = "Client Connected (Port 28000)";
                        numUsersLabel.Content = "True";

                        latestMessageReceivedTime.Content = DateTime.Now.ToString();

                        messagesReceived28000++;
                        numReceivedMessages.Content = messagesReceived28000 + messagesReceived27000;

                        latestMessage.Content = dataReceived;

                        if (dataReceived == "[Disconnect]")
                        {
                            serverStatusLabel.Content = "Client Disconnected (Port 28000)";
                        }
                    });

                    // forward the received data to port 27500
                    server27000.Send(data, data.Length, new IPEndPoint(clientEndPoint.Address, port27500));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling client connection on port 28000: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // function to save the data to file 
        private void SaveDataToFile(string data, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    writer.WriteLine(data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // properly close the servers 
        private void StopServers()
        {
            server27000.Close();
            server28000.Close();
        }

        // close the GUI and servers on close 
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            StopServers();
        }
    }
}
