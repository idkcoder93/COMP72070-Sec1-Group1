
namespace ClientInterface
{
    internal class ChatUser
    {
        private string v1;
        private int v2;

        public ChatUser(string v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        internal unsafe char* GetData()
        {
            throw new NotImplementedException();
        }

        internal void SetData(char[] newData, int v)
        {
            throw new NotImplementedException();
        }
    }
}