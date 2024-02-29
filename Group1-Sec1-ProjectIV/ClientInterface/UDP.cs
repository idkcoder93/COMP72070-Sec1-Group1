using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientInterface
{
    internal class UDP
    {
        private const string LocalhostIP = "127.0.0.1"; // hardcorded localhost IP address
        private const int PortNumber = 27000; // hardcoded port number

        public async Task SendUDPMessage(string message)
        {
            using (UdpClient udpClient = new UdpClient())
            {
                try
                {
                    byte[] sendBytes = Encoding.ASCII.GetBytes(message);
                    await udpClient.SendAsync(sendBytes, sendBytes.Length, LocalhostIP, PortNumber);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while sending UDP message: " + ex.Message);
                }
            }
        }

        public async Task<string?> ReceiveUDPMessage()
        {
            using (UdpClient udpClient = new UdpClient(PortNumber))
            {
                try
                {
                    UdpReceiveResult result = await udpClient.ReceiveAsync();
                    return Encoding.ASCII.GetString(result.Buffer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while receiving UDP message: " + ex.Message);
                    return null;
                }
            }
        }
    }
}
