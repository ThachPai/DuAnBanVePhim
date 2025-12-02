namespace Phim3.MainChinh
{
    partial class FormChonGhe
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
            flpGhe = new FlowLayoutPanel();
            lblTongTien = new Label();
            btnXacNhan = new Button();
            panel1 = new Panel();
            label1 = new Label();
            SuspendLayout();
            // 
            // flpGhe
            // 
            flpGhe.AutoScroll = true;
            flpGhe.Location = new Point(170, 27);
            flpGhe.Name = "flpGhe";
            flpGhe.Size = new Size(650, 428);
            flpGhe.TabIndex = 0;
            // 
            // lblTongTien
            // 
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTongTien.Location = new Point(470, 467);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new Size(71, 28);
            lblTongTien.TabIndex = 1;
            lblTongTien.Text = "0 VNĐ";
            // 
            // btnXacNhan
            // 
            btnXacNhan.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXacNhan.Location = new Point(429, 508);
            btnXacNhan.Name = "btnXacNhan";
            btnXacNhan.Size = new Size(148, 34);
            btnXacNhan.TabIndex = 2;
            btnXacNhan.Text = "Thanh toán";
            btnXacNhan.UseVisualStyleBackColor = true;
            btnXacNhan.Click += btnXacNhan_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Red;
            panel1.Location = new Point(902, 56);
            panel1.Name = "panel1";
            panel1.Size = new Size(64, 55);
            panel1.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(996, 74);
            label1.Name = "label1";
            label1.Size = new Size(82, 28);
            label1.TabIndex = 4;
            label1.Text = ": Đã đặt";
            // 
            // FormChonGhe
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1135, 596);
            Controls.Add(label1);
            Controls.Add(panel1);
            Controls.Add(btnXacNhan);
            Controls.Add(lblTongTien);
            Controls.Add(flpGhe);
            Name = "FormChonGhe";
            Text = "FormChonGhe";
            Load += FormChonGhe_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flpGhe;
        private Label lblTongTien;
        private Button btnXacNhan;
        private Panel panel1;
        private Label label1;
    }
}