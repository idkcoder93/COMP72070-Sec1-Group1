using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ClientInterface;
using System.Text;
using System.Net.Sockets; // Importing the namespace where ChatUser is defined


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

        //[TestMethod]
        //public void SetData_ValidData()
        //{
        //    // Arrange
        //    ChatUser user = new ChatUser("testUser", 10);
        //    var newData = new char[] { 'a', 'b', 'c' };
        //    var expectedDataAsString = "abc"; // String representation of the expected data

        //    // Act
        //    user.SetData(newData, 3); // Assuming SetData updates both the data and its size

        //    // Assert
        //    var actualDataAsString = user.GetDataAsString(); // Assuming GetData returns the char array or similar method
        //    Assert.AreEqual(expectedDataAsString, actualDataAsString);
        //}

        //[TestMethod]
        //public void SendMessage_WithEmptyMessage_ShouldShowMessageBox()
        //{
        //    // Arrange
        //    var form = new Form1();
        //    var mockMessageBox = new Mock<IMessageBoxWrapper>();
        //    form.SetMessageBoxWrapper(mockMessageBox.Object);

        //    // Act
        //    form.SendMessage();

        //    // Assert
        //    mockMessageBox.Verify(m => m.Show("Cannot send empty messages.", "Cannot Send"), Times.Once);
        //}

        //[TestMethod]
        //public void SendMessage_WithNonEmptyMessage()
        //{
        //    // Arrange
        //    var form = new Form1();
        //    var mockUdpClient = new Mock<IUdpClientWrapper>();
        //    form.SetUdpClientWrapper(mockUdpClient.Object);

        //    string testMessage = "test message";

        //    // Act
        //    form.SetTextBoxText(testMessage);
        //    form.SendMessage();
        //    string lastSentMessage = form.GetLastSentMessage(); // Get the last sent message


        //    // Assert
        //    Assert.AreEqual(lastSentMessage, testMessage);
        //}

    }
}
