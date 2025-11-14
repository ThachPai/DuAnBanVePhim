namespace Phim3
{
    partial class Form3
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
            panel1 = new Panel();
            label5 = new Label();
            pictureBox1 = new PictureBox();
            btnXacNhan = new Button();
            txtNewPass = new TextBox();
            label4 = new Label();
            btnGuiOTP = new Button();
            txtOTP = new TextBox();
            label3 = new Label();
            txtEmail = new TextBox();
            label2 = new Label();
            label1 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label5);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(btnXacNhan);
            panel1.Controls.Add(txtNewPass);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(btnGuiOTP);
            panel1.Controls.Add(txtOTP);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(txtEmail);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panel1.Location = new Point(0, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(573, 552);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(195, 88);
            label5.Name = "label5";
            label5.Size = new Size(173, 32);
            label5.TabIndex = 10;
            label5.Text = "PhimMoi.com";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.clapperboard;
            pictureBox1.Location = new Point(222, 10);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(103, 75);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // btnXacNhan
            // 
            btnXacNhan.BackColor = Color.Black;
            btnXacNhan.ForeColor = Color.White;
            btnXacNhan.Location = new Point(73, 494);
            btnXacNhan.Name = "btnXacNhan";
            btnXacNhan.Size = new Size(435, 51);
            btnXacNhan.TabIndex = 8;
            btnXacNhan.Text = "Xác nhận";
            btnXacNhan.UseVisualStyleBackColor = false;
            btnXacNhan.Click += btnXacNhan_Click;
            // 
            // txtNewPass
            // 
            txtNewPass.Location = new Point(73, 442);
            txtNewPass.Name = "txtNewPass";
            txtNewPass.Size = new Size(435, 34);
            txtNewPass.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(73, 402);
            label4.Name = "label4";
            label4.Size = new Size(139, 28);
            label4.TabIndex = 6;
            label4.Text = "Mật khẩu mới";
            // 
            // btnGuiOTP
            // 
            btnGuiOTP.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGuiOTP.Location = new Point(73, 231);
            btnGuiOTP.Name = "btnGuiOTP";
            btnGuiOTP.Size = new Size(145, 34);
            btnGuiOTP.TabIndex = 5;
            btnGuiOTP.Text = "Gửi mã OTP";
            btnGuiOTP.UseVisualStyleBackColor = true;
            btnGuiOTP.Click += btnGuiOTP_Click;
            // 
            // txtOTP
            // 
            txtOTP.Location = new Point(73, 343);
            txtOTP.Multiline = true;
            txtOTP.Name = "txtOTP";
            txtOTP.Size = new Size(150, 31);
            txtOTP.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(73, 304);
            label3.Name = "label3";
            label3.Size = new Size(138, 28);
            label3.TabIndex = 3;
            label3.Text = "Nhập mã OTP";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(73, 183);
            txtEmail.Multiline = true;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(435, 31);
            txtEmail.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(73, 152);
            label2.Name = "label2";
            label2.Size = new Size(60, 28);
            label2.TabIndex = 1;
            label2.Text = "Email";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(195, 120);
            label1.Name = "label1";
            label1.Size = new Size(184, 32);
            label1.TabIndex = 0;
            label1.Text = "Quên mật khẩu";
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(580, 559);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "Form3";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnGuiOTP;
        private TextBox txtOTP;
        private Label label3;
        private TextBox txtEmail;
        private Label label2;
        private Label label1;
        private Button btnXacNhan;
        private TextBox txtNewPass;
        private Label label4;
        private Label label5;
        private PictureBox pictureBox1;
    }
}