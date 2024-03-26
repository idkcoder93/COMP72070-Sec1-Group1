using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ClientInterface;
using System.Text;
using System.Net.Sockets;
using System.Net; // Importing the namespace where ChatUser is defined


namespace TestProject
{
    [TestClass]
    public class Client_UnitTest
    {
        [TestMethod]
        public void CLIENT001_ClientCanReceiveMessagesUsingUDP()
        {
            // Arrange
            var udpClientWrapperMock = new Mock<IUdpClientWrapper>();
            var autoResetEvent = new AutoResetEvent(false); // Synchronization event
            string expectedMessage = "Random message";
            byte[] messageBytes = Encoding.ASCII.GetBytes(expectedMessage);
            Form1 testForm = new Form1();
            testForm.SetUdpClientWrapper(udpClientWrapperMock.Object);

            udpClientWrapperMock.Setup(x => x.Send(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Callback((byte[] data, int length, string ip, int port) =>
                {
                    testForm.DisplayReceivedMessage(Encoding.ASCII.GetString(data, 0, length));
                    autoResetEvent.Set(); // Signal that the message has been processed
                });

            // Act
            udpClientWrapperMock.Object.Send(messageBytes, messageBytes.Length, "127.0.0.1", 27500);
            bool signalReceived = autoResetEvent.WaitOne(1000); // Wait for up to 1 second for the message to be processed

            // Assert
            Assert.IsTrue(signalReceived, "Signal not received within the expected timeframe, indicating the message was not processed.");

            string actualMessage = testForm.GetLastReceivedMessage();
            Assert.AreEqual(expectedMessage, actualMessage, "The received message does not match the expected message.");
        }

        [TestMethod]
        public void CLIENT002_ClientCanSendPacketsUsingUDP()
        {
            // Arrange
            var udpClientWrapperMock = new Mock<IUdpClientWrapper>();
            string expectedMessage = "Random message";
            byte[] messageBytes = Encoding.ASCII.GetBytes(expectedMessage);

            // Setup the mock to verify that the Send method is called with the correct parameters
            udpClientWrapperMock.Setup(x => x.Send(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Callback((byte[] data, int length, string ip, int port) =>
                {
                    string sentMessage = Encoding.ASCII.GetString(data, 0, length);
                    Assert.AreEqual(expectedMessage, sentMessage, "The sent message does not match the expected message.");
                });

            Form1 testForm = new Form1();
            testForm.SetUdpClientWrapper(udpClientWrapperMock.Object);

            // Act
            testForm.SendUdpMessage(expectedMessage); // This is a method would create to send the message using the UDP client

            // Assert
            udpClientWrapperMock.Verify(x => x.Send(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once(), "The message was not sent exactly once.");
        }

        [TestMethod]
        public async Task CLIENT003_ReceiveImagesFromServer()
        {
            // Arrange
            var tcpClientMock = new Mock<ITcpClientWrapper>();
            var networkStreamMock = new Mock<INetworkStream>();

            tcpClientMock.Setup(x => x.Connected).Returns(true);
            tcpClientMock.Setup(x => x.GetStream()).Returns(networkStreamMock.Object);

            // Simulate receiving an image length of 4, followed by the image data
            var sequence = new MockSequence();
            networkStreamMock.InSequence(sequence)
                .Setup(x => x.ReadAsync(It.IsAny<byte[]>(), 0, 4, It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
                {
                    BitConverter.GetBytes(4).CopyTo(buffer, offset);
                    return 4; // Number of bytes read (image length)
                });

            networkStreamMock.InSequence(sequence)
                .Setup(x => x.ReadAsync(It.IsAny<byte[]>(), 0, It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
                {
                    new byte[] { 1, 2, 3, 4 }.CopyTo(buffer, offset);
                    return 4; // Number of bytes read (image data)
                });

            var form = new Form1(tcpClientMock.Object); // Assuming Form1 accepts ITcpClientWrapper in its constructor

            // Act
            var receiveTask = form.ReceiveImagesFromServer(); // You might need to adjust the control flow for testing
            await Task.WhenAny(receiveTask, Task.Delay(1000)); // Timeout to prevent hanging in case of an error

            // Assert
            string expectedFilePath = @"C:\Users\codud\source\repos\COMP72070-Sec1-Group1\Group1-Sec1-ProjectIV\TestClient\TestImage\test.jpeg";

            // Ensure the file exists
            Assert.IsTrue(File.Exists(expectedFilePath), "The expected image file does not exist.");

            // Additional checks could include verifying file size, format, etc.
            // Example: Check the file is not empty
            var fileInfo = new FileInfo(expectedFilePath);
            Assert.IsTrue(fileInfo.Length > 0, "The image file is empty.");
        }

        [TestMethod]
        public async Task CLIENT004_ReceiveMessagesThroughUDP()
        {
            // Arrange
            var udpClientWrapperMock = new Mock<IUdpClientWrapper>();
            string expectedMessage = "Test message";
            byte[] messageBytes = Encoding.ASCII.GetBytes(expectedMessage);
            Form1 formUnderTest = new Form1();
            formUnderTest.SetUdpClientWrapper(udpClientWrapperMock.Object);

            udpClientWrapperMock.Setup(x => x.ReceiveAsync())
                .ReturnsAsync(// need to change /*new UdpReceiveResult(messageBytes, new IPEndPoint(IPAddress.Loopback, 0))*/); // Simulating message reception

            // Act
            await Task.Run(() => formUnderTest.StartReceivingMessages()); // Might need to adjust for actual async handling
            string actualMessage = formUnderTest.GetLastReceivedMessage();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage, "The received message does not match the expected message.");
        }
    }
}
