namespace ClientInterface
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.KeyPress += textBox1_KeyPress;
        }

        private void clientButton_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Prevent the Enter key from being processed by the TextBox
                e.Handled = true;

                // Get the text from the TextBox
                string newText = textBox1.Text;

                // Create a new chat bubble
                MeChatBubble meChatBubble1 = new MeChatBubble();

                // Set the text of the chat bubble
                MeChatBubble.SetBubbleText(meChatBubble1, newText);

                // Initializing UDP 
                UDP uDP = new UDP();   

                // Calculate the Y-position for the new chat bubble
                int newY = (chatContainerPanel.Controls.Count > 0) ? chatContainerPanel.Controls[chatContainerPanel.Controls.Count - 1].Bottom + 10 : 0;

                // Set the location of the new chat bubble
                meChatBubble1.Location = new Point(0, newY);

                // Add the bubble to the chat panel
                chatContainerPanel.Controls.Add(meChatBubble1);

                // Sending message to server
                _ = uDP.SendUDPMessage(newText);

                // Clear the TextBox
                textBox1.Clear();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
