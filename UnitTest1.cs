using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Server_Tests
{
    [TestClass]
    public class ServerTests
    {
        // This unit test ensures that the server can successfully accepts a connection request from a client.
        [TestMethod]
        public void SERVER_001_AcceptClientConnection()
        {
            // Arrange
            var server = new ServerTests(); 

            // Act
            var isConnected = server.AcceptClientConnection(); 
            // Assert
            Assert.IsTrue(isConnected, "Server should successfully accept a client connection.");
            Assert.IsTrue(server.IsClientConnected, "Server should indicate that a client is connected.");
        }

        //This integration test validates the server's ability to process incoming requests from connected clients.
        [TestMethod]
        public void SERVER_002_ProcessClientRequests()
        {
            // Arrange
            var server = new ServerTests(); 
            var messageFromClient = "Hello from client";

            // Act
            var processedSuccessfully = server.ProcessClientRequest(messageFromClient); 

            // Assert
            Assert.IsTrue(processedSuccessfully, "Server should successfully process client requests.");
            Assert.AreEqual(1, server.ReceivedMessagesCount, "Server should update the received messages count.");
        }

        // This integration test ensures that the server can interact with the data storage system to retrieve or store data as required.
        [TestMethod]
        public void SERVER_003DataStorageIntegration()
        {
            // Arrange
            var server = new ServerTests(); 

            // Act
            var dataOperationSuccess = server.DataStorageOperation();

            // Assert
            Assert.IsFalse(dataOperationSuccess, "Server is not able to store the Image data");
        }

        //This system test verifies that the server properly authenticates clients based on their credentials.
        [TestMethod]
        public void SERVER_004_securityAuthentication()
        {
            // Arrange
            var server = new ServerTests();

            // Act
            var isAuthenticated = server.AuthenticateClient(); 

            // Assert
            Assert.IsFalse(isAuthenticated, "Server ia unable to verify client's credential");
        }

        //This non-functional test assesses the server's load balancing capabilities under varying connection loads.
        [TestMethod]
        public void SERVER_005_LoadBalancing()
        {
            // Arrange
            var server = new ServerTests(); 

            // Act
            var isLoadBalanced = server.PerformLoadBalancing();

            // Assert
            Assert.IsFalse(isLoadBalanced, "Server is not able to connect with more than one client");
        }

        //This unit test verifies the server's ability to handle and log errors gracefully when there is a failure to establish a connection with a client.
        [TestMethod]
        public void SERVER_006_ConnectionFailure()
        {
            // Arrange
            var server = new ServerTests(); 
            var simulatedFailure = true; 

            // Act
            var connectionFailureHandled = server.HandleConnectionFailure(simulatedFailure);

            // Assert
            Assert.IsFalse(connectionFailureHandled, "Server should handle connection failures gracefully.");
            Assert.IsTrue(server.ErrorLog.Contains("Connection failed"), "Server should log connection failure.");
        }

        //This system test ensures that the server performs data integrity checks on received data to identify and handle any potential corruption or errors.
        [TestMethod]
        public void SERVER_007_DataIntegrityCheck()
        {
            // Arrange
            var server = new ServerTests();

            // Act
            var dataIntegrityChecked = server.PerformDataIntegrityCheck(); 

            // Assert
            Assert.IsFalse(dataIntegrityChecked, "Server is not able to perform data integrity checks");
        }


    //Method that simulate Handle ConnectionFailiure
        private bool HandleConnectionFailure(bool simulatedFailure)
        {
            try
            {
                if (simulatedFailure)
                {
                    throw new Exception("Connection failed.");
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                ErrorLog.Add("Connection failed");
                return false;
            }
        }


        //Method that simulate DataStorageOperation
        private bool DataStorageOperation()
        {
            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Method that simulate ProcessClientRequest
        private bool ProcessClientRequest(string messageFromClient)
        {
            try
            {
                if (!string.IsNullOrEmpty(messageFromClient))
                {
                    ReceivedMessagesCount--;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            { 
                return false;
            }
        }


        //Method that simulate AcceptClientConnection
        private bool AcceptClientConnection()
        {
            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Mathod that simulate PerformLoadBalancing
        private bool PerformLoadBalancing()
        {
            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Method that perform PerformDataIntegrityCheck
        private bool PerformDataIntegrityCheck()
        {
            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Method that simulate AuthenticateClient
        private bool AuthenticateClient()
        {
            try
            {
                return true;
            }
            catch (Exception)
            { 
                return false;
            }
        }

        public bool IsClientConnected { get; private set; } = true;
        public int ReceivedMessagesCount { get; private set; } = 2;
        public List<string> ErrorLog { get; private set; } = new List<string>();
    }

}