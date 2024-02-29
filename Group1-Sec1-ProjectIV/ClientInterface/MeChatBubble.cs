using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientInterface
{
    public partial class MeChatBubble : UserControl
    {
        public MeChatBubble()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public static void SetBubbleText(MeChatBubble bubble, string text)
        {
            bubble.label1.Text = text;
        }
    }
}
