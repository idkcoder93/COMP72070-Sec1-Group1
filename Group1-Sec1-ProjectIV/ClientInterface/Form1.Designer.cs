namespace ClientInterface
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            sideBar = new FlowLayoutPanel();
            clientButton = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            textBox1 = new TextBox();
            button1 = new Button();
            chatBubbleControl1 = new YouChatBubble();
            meChatBubble1 = new MeChatBubble();
            sideBar.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // sideBar
            // 
            sideBar.BackColor = SystemColors.AppWorkspace;
            sideBar.Controls.Add(clientButton);
            sideBar.Dock = DockStyle.Left;
            sideBar.FlowDirection = FlowDirection.TopDown;
            sideBar.Location = new Point(0, 0);
            sideBar.Name = "sideBar";
            sideBar.Size = new Size(217, 1038);
            sideBar.TabIndex = 0;
            // 
            // clientButton
            // 
            clientButton.BackColor = SystemColors.AppWorkspace;
            clientButton.FlatAppearance.BorderSize = 0;
            clientButton.FlatStyle = FlatStyle.Flat;
            clientButton.Image = Properties.Resources.team;
            clientButton.ImageAlign = ContentAlignment.MiddleLeft;
            clientButton.Location = new Point(5, 0);
            clientButton.Margin = new Padding(5, 0, 0, 0);
            clientButton.Name = "clientButton";
            clientButton.Padding = new Padding(3);
            clientButton.Size = new Size(217, 117);
            clientButton.TabIndex = 1;
            clientButton.Text = "  client#1";
            clientButton.UseVisualStyleBackColor = false;
            clientButton.Click += clientButton_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(textBox1);
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.Location = new Point(217, 907);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5, 10, 20, 30);
            flowLayoutPanel1.Size = new Size(1917, 131);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Segoe UI Variable Display", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(15, 20);
            textBox1.Margin = new Padding(10, 10, 3, 3);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Type here...";
            textBox1.Size = new Size(1741, 94);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.BackColor = SystemColors.Control;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Image = Properties.Resources.paper_clip;
            button1.Location = new Point(1771, 13);
            button1.Margin = new Padding(12, 3, 3, 3);
            button1.Name = "button1";
            button1.Size = new Size(117, 101);
            button1.TabIndex = 2;
            button1.UseVisualStyleBackColor = false;
            // 
            // chatBubbleControl1
            // 
            chatBubbleControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chatBubbleControl1.BackColor = Color.Transparent;
            chatBubbleControl1.Location = new Point(217, 0);
            chatBubbleControl1.Margin = new Padding(6, 8, 6, 8);
            chatBubbleControl1.Name = "chatBubbleControl1";
            chatBubbleControl1.Padding = new Padding(0, 10, 10, 13);
            chatBubbleControl1.Size = new Size(1917, 187);
            chatBubbleControl1.TabIndex = 2;
            // 
            // meChatBubble1
            // 
            meChatBubble1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            meChatBubble1.AutoSize = true;
            meChatBubble1.BackColor = Color.Transparent;
            meChatBubble1.Location = new Point(217, 194);
            meChatBubble1.Margin = new Padding(6, 8, 6, 8);
            meChatBubble1.MinimumSize = new Size(0, 101);
            meChatBubble1.Name = "meChatBubble1";
            meChatBubble1.Padding = new Padding(10, 10, 10, 13);
            meChatBubble1.Size = new Size(1917, 187);
            meChatBubble1.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2134, 1038);
            Controls.Add(meChatBubble1);
            Controls.Add(chatBubbleControl1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(sideBar);
            Name = "Form1";
            Text = "Form1";
            sideBar.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel sideBar;
        private Button clientButton;
        private FlowLayoutPanel flowLayoutPanel1;
        private TextBox textBox1;
        private Button button1;
        private YouChatBubble chatBubbleControl1;
        private MeChatBubble meChatBubble1;
    }
}
