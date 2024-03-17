namespace ClientInterface
{
    partial class YouChatBubble
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            panel2 = new Panel();
            pictureBox1 = new PictureBox();
            chatLabel = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 10);
            panel1.Margin = new Padding(6, 8, 6, 8);
            panel1.Name = "panel1";
            panel1.Size = new Size(98, 163);
            panel1.TabIndex = 2;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(pictureBox1);
            panel2.Location = new Point(9, 0);
            panel2.Margin = new Padding(6, 8, 6, 8);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(2, 3, 2, 3);
            panel2.Size = new Size(74, 83);
            panel2.TabIndex = 1;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Navy;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = ClientSync.Properties.Resources.chat_icon;
            pictureBox1.Location = new Point(2, 3);
            pictureBox1.Margin = new Padding(6, 8, 6, 8);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(70, 77);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // chatLabel
            // 
            chatLabel.BackColor = Color.FromArgb(244, 244, 244);
            chatLabel.Cursor = Cursors.IBeam;
            chatLabel.Dock = DockStyle.Top;
            chatLabel.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chatLabel.ForeColor = SystemColors.ControlDarkDark;
            chatLabel.Location = new Point(98, 10);
            chatLabel.Margin = new Padding(6);
            chatLabel.Name = "chatLabel";
            chatLabel.Padding = new Padding(6, 8, 6, 8);
            chatLabel.Size = new Size(1802, 154);
            chatLabel.TabIndex = 3;
            // 
            // YouChatBubble
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(chatLabel);
            Controls.Add(panel1);
            Margin = new Padding(6, 8, 6, 8);
            Name = "YouChatBubble";
            Padding = new Padding(0, 10, 10, 13);
            Size = new Size(1910, 186);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        protected System.Windows.Forms.Label chatLabel;
    }
}
