namespace ClientInterface
{
    partial class MeChatBubble
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
            label1 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(1802, 10);
            panel1.Margin = new Padding(6, 8, 6, 8);
            panel1.Name = "panel1";
            panel1.Size = new Size(98, 163);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(pictureBox1);
            panel2.Location = new Point(17, 0);
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
            // label1
            // 
            label1.BackColor = Color.DodgerBlue;
            label1.Cursor = Cursors.IBeam;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(10, 10);
            label1.Margin = new Padding(6);
            label1.Name = "label1";
            label1.Padding = new Padding(10, 8, 6, 8);
            label1.Size = new Size(1792, 163);
            label1.TabIndex = 1;
            label1.Click += label1_Click;
            // 
            // MeChatBubble
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.Transparent;
            Controls.Add(label1);
            Controls.Add(panel1);
            Margin = new Padding(6, 8, 6, 8);
            MinimumSize = new Size(0, 101);
            Name = "MeChatBubble";
            Padding = new Padding(10, 10, 10, 13);
            Size = new Size(1910, 186);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.PictureBox pictureBox1;
    protected System.Windows.Forms.Label label1;
    private System.Windows.Forms.Panel panel2;
    }
}
