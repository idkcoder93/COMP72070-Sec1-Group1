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
    public partial class YouChatBubble : UserControl
    {
        public YouChatBubble()
        {
            InitializeComponent();
        }

        public static void SetBubbleText(YouChatBubble bubble, string text)
        {
            bubble.chatLabel.Text = text;
        }
    }
}
