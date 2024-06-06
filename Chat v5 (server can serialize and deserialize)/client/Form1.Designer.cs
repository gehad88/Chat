namespace client
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
            this.ContentBox = new System.Windows.Forms.RichTextBox();
            this.MessageBox = new System.Windows.Forms.TextBox();
            this.Connect = new System.Windows.Forms.Button();
            this.Send = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.Encription = new System.Windows.Forms.Button();
            this.Decription = new System.Windows.Forms.Button();
            this.Serialize = new System.Windows.Forms.Button();
            this.DIR = new System.Windows.Forms.Button();
            this.GET = new System.Windows.Forms.Button();
            this.Deserialize = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // ContentBox
            // 
            this.ContentBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContentBox.DetectUrls = false;
            this.ContentBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ContentBox.Location = new System.Drawing.Point(15, 16);
            this.ContentBox.Margin = new System.Windows.Forms.Padding(4);
            this.ContentBox.Name = "ContentBox";
            this.ContentBox.Size = new System.Drawing.Size(607, 457);
            this.ContentBox.TabIndex = 0;
            this.ContentBox.Text = "";
            // 
            // MessageBox
            // 
            this.MessageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MessageBox.Enabled = false;
            this.MessageBox.Location = new System.Drawing.Point(14, 481);
            this.MessageBox.Margin = new System.Windows.Forms.Padding(4);
            this.MessageBox.Multiline = true;
            this.MessageBox.Name = "MessageBox";
            this.MessageBox.Size = new System.Drawing.Size(608, 57);
            this.MessageBox.TabIndex = 1;
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(628, 511);
            this.Connect.Margin = new System.Windows.Forms.Padding(4);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(88, 28);
            this.Connect.TabIndex = 2;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // Send
            // 
            this.Send.Enabled = false;
            this.Send.Location = new System.Drawing.Point(722, 511);
            this.Send.Margin = new System.Windows.Forms.Padding(4);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(88, 28);
            this.Send.TabIndex = 3;
            this.Send.Text = "Send";
            this.Send.UseVisualStyleBackColor = true;
            this.Send.Click += new System.EventHandler(this.Send_Click);
            // 
            // Close
            // 
            this.Close.Enabled = false;
            this.Close.Location = new System.Drawing.Point(817, 511);
            this.Close.Margin = new System.Windows.Forms.Padding(4);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(88, 28);
            this.Close.TabIndex = 4;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(639, 16);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(282, 256);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // Encription
            // 
            this.Encription.Location = new System.Drawing.Point(630, 477);
            this.Encription.Margin = new System.Windows.Forms.Padding(4);
            this.Encription.Name = "Encription";
            this.Encription.Size = new System.Drawing.Size(88, 28);
            this.Encription.TabIndex = 7;
            this.Encription.Text = "Encription";
            this.Encription.UseVisualStyleBackColor = true;
            // 
            // Decription
            // 
            this.Decription.Location = new System.Drawing.Point(721, 477);
            this.Decription.Margin = new System.Windows.Forms.Padding(4);
            this.Decription.Name = "Decription";
            this.Decription.Size = new System.Drawing.Size(88, 28);
            this.Decription.TabIndex = 8;
            this.Decription.Text = "Decription";
            this.Decription.UseVisualStyleBackColor = true;
            // 
            // Serialize
            // 
            this.Serialize.Location = new System.Drawing.Point(817, 475);
            this.Serialize.Margin = new System.Windows.Forms.Padding(4);
            this.Serialize.Name = "Serialize";
            this.Serialize.Size = new System.Drawing.Size(88, 28);
            this.Serialize.TabIndex = 9;
            this.Serialize.Text = "Serialize";
            this.Serialize.UseVisualStyleBackColor = true;
            this.Serialize.Click += new System.EventHandler(this.Serialize_Click);
            // 
            // DIR
            // 
            this.DIR.Location = new System.Drawing.Point(630, 441);
            this.DIR.Margin = new System.Windows.Forms.Padding(4);
            this.DIR.Name = "DIR";
            this.DIR.Size = new System.Drawing.Size(88, 28);
            this.DIR.TabIndex = 10;
            this.DIR.Text = "DIR";
            this.DIR.UseVisualStyleBackColor = true;
            // 
            // GET
            // 
            this.GET.Location = new System.Drawing.Point(722, 441);
            this.GET.Margin = new System.Windows.Forms.Padding(4);
            this.GET.Name = "GET";
            this.GET.Size = new System.Drawing.Size(88, 28);
            this.GET.TabIndex = 11;
            this.GET.Text = "GET";
            this.GET.UseVisualStyleBackColor = true;
            // 
            // Deserialize
            // 
            this.Deserialize.Location = new System.Drawing.Point(817, 441);
            this.Deserialize.Margin = new System.Windows.Forms.Padding(4);
            this.Deserialize.Name = "Deserialize";
            this.Deserialize.Size = new System.Drawing.Size(88, 28);
            this.Deserialize.TabIndex = 12;
            this.Deserialize.Text = "Deserialize";
            this.Deserialize.UseVisualStyleBackColor = true;
            this.Deserialize.Click += new System.EventHandler(this.Deserialize_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 554);
            this.Controls.Add(this.Deserialize);
            this.Controls.Add(this.GET);
            this.Controls.Add(this.DIR);
            this.Controls.Add(this.Serialize);
            this.Controls.Add(this.Decription);
            this.Controls.Add(this.Encription);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.Send);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.MessageBox);
            this.Controls.Add(this.ContentBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "NS TCP Client";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox ContentBox;
        private System.Windows.Forms.TextBox MessageBox;
        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.Button Send;
        private System.Windows.Forms.Button Close;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button Encription;
        private System.Windows.Forms.Button Decription;
        private System.Windows.Forms.Button Serialize;
        private System.Windows.Forms.Button DIR;
        private System.Windows.Forms.Button GET;
        private System.Windows.Forms.Button Deserialize;
    }
}

