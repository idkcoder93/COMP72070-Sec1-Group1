namespace ClientSync
{
    partial class ImageLinkLabel
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
            ImageLink = new LinkLabel();
            SuspendLayout();
            // 
            // linkLabel1
            // 
            ImageLink.AutoSize = true;
            ImageLink.Location = new Point(14, 15);
            ImageLink.Name = "linkLabel1";
            ImageLink.RightToLeft = RightToLeft.Yes;
            ImageLink.Size = new Size(128, 32);
            ImageLink.TabIndex = 0;
            ImageLink.TabStop = true;
            ImageLink.Text = "something";
            ImageLink.LinkClicked += ImageLinkClicked;
            // 
            // ImageLinkLabel
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ImageLink);
            Name = "ImageLinkLabel";
            Size = new Size(377, 101);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private LinkLabel ImageLink;
    }
}
