namespace RemoteKeyboardClient
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ipBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.keycodeBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(13, 13);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(178, 20);
            this.ipBox.TabIndex = 0;
            this.ipBox.Text = "Phone IP";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(197, 10);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 1;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // keycodeBox
            // 
            this.keycodeBox.Location = new System.Drawing.Point(13, 66);
            this.keycodeBox.Name = "keycodeBox";
            this.keycodeBox.Size = new System.Drawing.Size(100, 20);
            this.keycodeBox.TabIndex = 2;
            this.keycodeBox.Text = "Keycode";
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(120, 66);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 23);
            this.sendButton.TabIndex = 3;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 232);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.keycodeBox);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.ipBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ipBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox keycodeBox;
        private System.Windows.Forms.Button sendButton;
    }
}

