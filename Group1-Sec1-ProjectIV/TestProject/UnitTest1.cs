namespace ClientInterface

//namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SetData_ValidData()
        {
            // Arrange
            var user = new ChatUser("testUser", 10);
            var newData = new char[] { 'a', 'b', 'c' };
            var expectedDataAsString = "abc"; // String representation of the expected data

            // Act
            user.SetData(newData, 3); // Assuming SetData updates both the data and its size

            // Assert
            var actualDataAsString = new string(user.GetData()); // Assuming GetData returns the char array or similar method
            Assert.AreEqual(expectedDataAsString, actualDataAsString);
        }

    }
}