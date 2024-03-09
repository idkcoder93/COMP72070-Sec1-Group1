using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ClientInterface; // Importing the namespace where ChatUser is defined

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SetData_ValidData()
        {
            // Arrange
            ChatUser user = new ChatUser("testUser", 10);
            var newData = new char[] { 'a', 'b', 'c' };
            var expectedDataAsString = "abc"; // String representation of the expected data

            // Act
            user.SetData(newData, 3); // Assuming SetData updates both the data and its size

            // Assert
            var actualDataAsString = user.GetDataAsString(); // Assuming GetData returns the char array or similar method
            Assert.AreEqual(expectedDataAsString, actualDataAsString);
        }

        [TestMethod]
        public void SendMessage_WithEmptyMessage_ShouldShowMessageBox()
        {
            // Arrange
            var form = new Form1();
            var mockMessageBox = new Mock<IMessageBoxWrapper>();
            form.SetMessageBoxWrapper(mockMessageBox.Object);

            // Act
            form.SendMessage();

            // Assert
            mockMessageBox.Verify(m => m.Show("Cannot send empty messages.", "Cannot Send"), Times.Once);
        }

        [TestMethod]
        public void SendMessage_WithNonEmptyMessage()
        {
            // Arrange
            var form = new Form1();
            var mockUdpClient = new Mock<IUdpClientWrapper>();
            form.SetUdpClientWrapper(mockUdpClient.Object);

            string testMessage = "test message";

            // Act
            form.SetTextBoxText(testMessage);
            form.SendMessage();
            string lastSentMessage = form.GetLastSentMessage(); // Get the last sent message
           

            // Assert
            Assert.AreEqual(lastSentMessage, testMessage);
        }
    }
}
