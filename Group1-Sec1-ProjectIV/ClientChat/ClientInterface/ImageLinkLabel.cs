using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientSync
{
    public partial class ImageLinkLabel : UserControl
    {
        public ImageLinkLabel()
        {
            InitializeComponent();
        }

        internal void setTextLink(string text)
        {
            ImageLink.Text = text;
        }

        private void ImageLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string directory = System.Windows.Forms.Application.StartupPath;
            string relativePath = Path.Combine(directory, "image.jpeg");

            try
            {
                if (File.Exists(relativePath))
                {
                    var processStartInfo = new ProcessStartInfo { FileName = relativePath };
                    processStartInfo.UseShellExecute = true;
                    Process.Start(processStartInfo);
                }
                else
                {
                    MessageBox.Show("The specified file does not exist.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open file: " + ex.Message);
            }
        }
    }
}
