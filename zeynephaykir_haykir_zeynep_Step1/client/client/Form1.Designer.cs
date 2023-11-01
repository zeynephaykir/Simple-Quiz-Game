using System.Windows.Forms;

namespace client
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
            this.Logs = new System.Windows.Forms.RichTextBox();
            this.NameText = new System.Windows.Forms.TextBox();
            this.IPText = new System.Windows.Forms.TextBox();
            this.AnswerText = new System.Windows.Forms.TextBox();
            this.PortText = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.SendButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Logs
            // 
            this.Logs.Location = new System.Drawing.Point(345, 10);
            this.Logs.Name = "Logs";
            this.Logs.Size = new System.Drawing.Size(505, 192);
            this.Logs.TabIndex = 0;
            this.Logs.Text = "";
            this.Logs.TextChanged += new System.EventHandler(this.Logs_TextChanged);
            // 
            // NameText
            // 
            this.NameText.Location = new System.Drawing.Point(81, 50);
            this.NameText.Name = "NameText";
            this.NameText.Size = new System.Drawing.Size(214, 20);
            this.NameText.TabIndex = 1;
            // 
            // IPText
            // 
            this.IPText.Location = new System.Drawing.Point(81, 115);
            this.IPText.Name = "IPText";
            this.IPText.Size = new System.Drawing.Size(214, 20);
            this.IPText.TabIndex = 2;
            // 
            // AnswerText
            // 
            this.AnswerText.Location = new System.Drawing.Point(380, 234);
            this.AnswerText.Name = "AnswerText";
            this.AnswerText.Size = new System.Drawing.Size(421, 20);
            this.AnswerText.TabIndex = 3;
            this.AnswerText.TextChanged += new System.EventHandler(this.AnswerText_TextChanged);
            // 
            // PortText
            // 
            this.PortText.Location = new System.Drawing.Point(81, 182);
            this.PortText.Name = "PortText";
            this.PortText.Size = new System.Drawing.Size(214, 20);
            this.PortText.TabIndex = 4;
            this.PortText.TextChanged += new System.EventHandler(this.PortText_TextChanged);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(189, 234);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(64, 20);
            this.ConnectButton.TabIndex = 5;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(807, 234);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(64, 20);
            this.SendButton.TabIndex = 6;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "IP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Port:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(329, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Answer:";
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Enabled = false;
            this.DisconnectButton.Location = new System.Drawing.Point(81, 234);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(75, 20);
            this.DisconnectButton.TabIndex = 11;
            this.DisconnectButton.Text = "Disconnect";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Click += new System.EventHandler(this.Disconnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 278);
            this.Controls.Add(this.DisconnectButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.PortText);
            this.Controls.Add(this.AnswerText);
            this.Controls.Add(this.IPText);
            this.Controls.Add(this.NameText);
            this.Controls.Add(this.Logs);
            this.Name = "Form1";
            this.Text = "Player";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RichTextBox Logs;
        private TextBox NameText;
        private TextBox IPText;
        private TextBox AnswerText;
        private TextBox PortText;
        private Button ConnectButton;
        private Button SendButton;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button DisconnectButton;
    }
}