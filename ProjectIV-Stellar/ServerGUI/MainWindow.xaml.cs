using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServerGUI
{
    public partial class MainWindow : Window
    {
        private TcpListener server;
        private Thread listenThread;

        public MainWindow()
        {
            InitializeComponent();
            StartServer();
        }

        private void StartServer()
        {
            try
            {
                server = new TcpListener(IPAddress.Any, 27000);
                server.Start();

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
                    TcpClient client = server.AcceptTcpClient();

                    Thread clientThread = new Thread(HandleClient);
                    clientThread.Start(client);
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Error accepting client connection: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void HandleClient(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;

            Dispatcher.Invoke(() =>
            {
                serverStatusLabel.Content = "Client Connected";
            });

            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    // Handle the received data here, you can update UI elements or perform any other operations.
                    // For example: receivedMessagesListBox.Items.Add(dataReceived);

                    // Echo the message back to the client (optional)
                    byte[] response = Encoding.ASCII.GetBytes($"Server received: {dataReceived}");
                    stream.Write(response, 0, response.Length);
                }

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling client connection: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StopServer()
        {
            server.Stop();
            listenThread.Abort();
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
