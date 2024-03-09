using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientInterface; // Importing the namespace where ChatUser is defined

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Now you can use the ChatUser class here
            ChatUser user = new ChatUser("Alex Fixed it", 126);
            // Your test logic here
        }
    }
}
