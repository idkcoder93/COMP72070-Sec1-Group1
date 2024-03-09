using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ClientInterface
{
    public class ChatUser
    {
        private string username;
        private char[] data; 

        public ChatUser(string username, int dataSize)
        {
            this.username = username;
            data = new char[dataSize]; // Initialize data array
        }

        public void SetData(char[] newData, int dataSize)
        {
            data = new char[dataSize]; // Set the data array to the new data

            if (data != null)
            {
                Array.Copy(newData, data, dataSize);
                //data[dataSize] = '\0';
            }
            else
            {
                Console.WriteLine("Error allocating data");
            }
        }

        public ChatUser(char[] src)
        {
            int dataSize = src.Length;
            int usernameLength = username.Length;

            data = new char[usernameLength + dataSize]; // Allocate memory for data array

            // Copy the username characters into the data array
            Array.Copy(username.ToCharArray(), data, usernameLength);

            // Copy the src data into the data array starting after the username
            Array.Copy(src, 0, data, usernameLength, dataSize);
        }

        public string GetDataAsString()
        {
            return new string(data); // Converts the character array to string
        }
    }
}
