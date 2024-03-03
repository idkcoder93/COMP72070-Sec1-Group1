using ServerGUI.MVVM.View;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace ServerGUI
{
    public partial class MainWindow : Window
    {
        private UdpClient server;
        private Thread listenThread;
        private const int port = 27000;
        private int messagesReceived = 0; 

        public MainWindow()
        {
            InitializeComponent();
            StartServer();
        }

        private void StartServer()
        {
            try
            {
                server = new UdpClient(port);
                listenThread = new Thread(ListenForClients);
                listenThread.Start();

                // Update UI element indicating that the server has started
                Dispatcher.Invoke(() =>
                {
                    serverStatusLabel.Content = "Server started";
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting server: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListenForClients()
        {
            try
            {
                while (true)
                {
                    IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] clientData = server.Receive(ref clientEndPoint);

                    Thread clientThread = new Thread(() => HandleClient(clientData, clientEndPoint));
                    clientThread.Start();
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Error receiving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void HandleClient(byte[] data, IPEndPoint clientEndPoint)
        {
            try
            {
                string dataReceived = Encoding.ASCII.GetString(data);

                Dispatcher.Invoke(() =>
                {
                    serverStatusLabel.Content = "Client Connected";
                    numUsersLabel.Content = "1";

                    messagesReceived++;
                    numReceivedMessages.Content = messagesReceived;

                    latestMessage.Content = dataReceived; 
                });



                // Handle the received data here, you can update UI elements or perform any other operations.
                // For example: receivedMessagesListBox.Items.Add(dataReceived);

                // Echo the message back to the client (optional)
                byte[] response = Encoding.ASCII.GetBytes($"Server received: {dataReceived}");
                server.Send(response, response.Length, clientEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling client connection: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StopServer()
        {
            server.Close();
            // You can update UI elements to indicate that the server has stopped.
        }

        // Remember to handle closing of the main window to ensure the server is properly stopped.
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            StopServer();
        }
    }
}
