namespace OAIP3laba
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
            pictureBox = new PictureBox();
            textBox = new TextBox();
            ListBox = new ListBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(12, 223);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(537, 449);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            // 
            // textBox
            // 
            textBox.Location = new Point(12, 31);
            textBox.Name = "textBox";
            textBox.Size = new Size(537, 26);
            textBox.TabIndex = 1;
            // 
            // ListBox
            // 
            ListBox.FormattingEnabled = true;
            ListBox.Location = new Point(12, 68);
            ListBox.Name = "ListBox";
            ListBox.Size = new Size(537, 137);
            ListBox.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1358, 684);
            Controls.Add(ListBox);
            Controls.Add(textBox);
            Controls.Add(pictureBox);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox;
        private TextBox textBox;
        private ListBox ListBox;
    }
}
