namespace server
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
            this.port_label = new System.Windows.Forms.Label();
            this.path_label = new System.Windows.Forms.Label();
            this.path_button = new System.Windows.Forms.Button();
            this.listen_button = new System.Windows.Forms.Button();
            this.path_textbox = new System.Windows.Forms.TextBox();
            this.port_textBox = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.quiz_question_textbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // port_label
            // 
            this.port_label.AutoSize = true;
            this.port_label.Location = new System.Drawing.Point(42, 51);
            this.port_label.Name = "port_label";
            this.port_label.Size = new System.Drawing.Size(38, 17);
            this.port_label.TabIndex = 0;
            this.port_label.Text = "Port:";
            // 
            // path_label
            // 
            this.path_label.AutoSize = true;
            this.path_label.Location = new System.Drawing.Point(16, 18);
            this.path_label.Name = "path_label";
            this.path_label.Size = new System.Drawing.Size(67, 17);
            this.path_label.TabIndex = 1;
            this.path_label.Text = "Quiz File:";
            this.path_label.Click += new System.EventHandler(this.path_label_Click);
            // 
            // path_button
            // 
            this.path_button.Location = new System.Drawing.Point(476, 15);
            this.path_button.Name = "path_button";
            this.path_button.Size = new System.Drawing.Size(75, 23);
            this.path_button.TabIndex = 2;
            this.path_button.Text = "PATH";
            this.path_button.UseVisualStyleBackColor = true;
            this.path_button.Click += new System.EventHandler(this.path_button_Click);
            // 
            // listen_button
            // 
            this.listen_button.Font = new System.Drawing.Font("Georgia", 15F);
            this.listen_button.Location = new System.Drawing.Point(577, 45);
            this.listen_button.Name = "listen_button";
            this.listen_button.Size = new System.Drawing.Size(294, 40);
            this.listen_button.TabIndex = 3;
            this.listen_button.Text = "Activate";
            this.listen_button.UseVisualStyleBackColor = true;
            this.listen_button.Click += new System.EventHandler(this.listen_button_Click);
            // 
            // path_textbox
            // 
            this.path_textbox.Enabled = false;
            this.path_textbox.Location = new System.Drawing.Point(86, 15);
            this.path_textbox.Name = "path_textbox";
            this.path_textbox.Size = new System.Drawing.Size(384, 22);
            this.path_textbox.TabIndex = 4;
            this.path_textbox.TextChanged += new System.EventHandler(this.path_textbox_TextChanged);
            // 
            // port_textBox
            // 
            this.port_textBox.Location = new System.Drawing.Point(86, 48);
            this.port_textBox.Name = "port_textBox";
            this.port_textBox.Size = new System.Drawing.Size(130, 22);
            this.port_textBox.TabIndex = 5;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Baskerville Old Face", 7.8F);
            this.richTextBox1.Location = new System.Drawing.Point(12, 91);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(859, 232);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(574, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Number of Questions";
            // 
            // quiz_question_textbox
            // 
            this.quiz_question_textbox.Location = new System.Drawing.Point(758, 15);
            this.quiz_question_textbox.Name = "quiz_question_textbox";
            this.quiz_question_textbox.Size = new System.Drawing.Size(113, 22);
            this.quiz_question_textbox.TabIndex = 8;
            this.quiz_question_textbox.TextChanged += new System.EventHandler(this.quiz_question_textbox_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 335);
            this.Controls.Add(this.quiz_question_textbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.port_textBox);
            this.Controls.Add(this.path_textbox);
            this.Controls.Add(this.listen_button);
            this.Controls.Add(this.path_button);
            this.Controls.Add(this.path_label);
            this.Controls.Add(this.port_label);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label port_label;
        private System.Windows.Forms.Label path_label;
        private System.Windows.Forms.Button path_button;
        private System.Windows.Forms.Button listen_button;
        private System.Windows.Forms.TextBox path_textbox;
        private System.Windows.Forms.TextBox port_textBox;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox quiz_question_textbox;
    }
}

