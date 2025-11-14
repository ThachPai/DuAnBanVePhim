namespace Phim3.PhanDangNhap
{
    partial class CapNhatcs
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
            label1 = new Label();
            txtTenPhim = new TextBox();
            label2 = new Label();
            label3 = new Label();
            cbbTheLoai = new ComboBox();
            label4 = new Label();
            txtThoiLuong = new TextBox();
            btnHuy = new Button();
            btnLuu = new Button();
            label5 = new Label();
            txtGiaTien = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 70);
            label1.Name = "label1";
            label1.Size = new Size(97, 28);
            label1.TabIndex = 0;
            label1.Text = "Tên phim";
            // 
            // txtTenPhim
            // 
            txtTenPhim.Location = new Point(12, 101);
            txtTenPhim.Multiline = true;
            txtTenPhim.Name = "txtTenPhim";
            txtTenPhim.Size = new Size(280, 43);
            txtTenPhim.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 159);
            label2.Name = "label2";
            label2.Size = new Size(84, 28);
            label2.TabIndex = 2;
            label2.Text = "Thể loại";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 257);
            label3.Name = "label3";
            label3.Size = new Size(111, 28);
            label3.TabIndex = 4;
            label3.Text = "Thời lượng";
            // 
            // cbbTheLoai
            // 
            cbbTheLoai.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbbTheLoai.FormattingEnabled = true;
            cbbTheLoai.Items.AddRange(new object[] { "Hành động", "Chính kịch", "Kinh dị", "Hài kịch", "Khoa học viễn tưởng" });
            cbbTheLoai.Location = new Point(12, 201);
            cbbTheLoai.Name = "cbbTheLoai";
            cbbTheLoai.Size = new Size(280, 36);
            cbbTheLoai.TabIndex = 6;
            cbbTheLoai.SelectedIndexChanged += cbbTheLoai_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(-1, 9);
            label4.Name = "label4";
            label4.Size = new Size(184, 32);
            label4.TabIndex = 7;
            label4.Text = "Chỉnh sửa phim";
            // 
            // txtThoiLuong
            // 
            txtThoiLuong.Location = new Point(12, 288);
            txtThoiLuong.Multiline = true;
            txtThoiLuong.Name = "txtThoiLuong";
            txtThoiLuong.Size = new Size(280, 43);
            txtThoiLuong.TabIndex = 8;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.Black;
            btnHuy.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(35, 460);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(88, 42);
            btnHuy.TabIndex = 9;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            // 
            // btnLuu
            // 
            btnLuu.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLuu.Location = new Point(139, 460);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(144, 42);
            btnLuu.TabIndex = 10;
            btnLuu.Text = "Xác nhận";
            btnLuu.UseVisualStyleBackColor = true;
            btnLuu.Click += btnLuu_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(12, 334);
            label5.Name = "label5";
            label5.Size = new Size(82, 28);
            label5.TabIndex = 11;
            label5.Text = "Giá tiền";
            // 
            // txtGiaTien
            // 
            txtGiaTien.Location = new Point(12, 365);
            txtGiaTien.Multiline = true;
            txtGiaTien.Name = "txtGiaTien";
            txtGiaTien.Size = new Size(280, 43);
            txtGiaTien.TabIndex = 12;
            // 
            // CapNhatcs
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(370, 573);
            Controls.Add(txtGiaTien);
            Controls.Add(label5);
            Controls.Add(btnLuu);
            Controls.Add(btnHuy);
            Controls.Add(txtThoiLuong);
            Controls.Add(label4);
            Controls.Add(cbbTheLoai);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtTenPhim);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "CapNhatcs";
            Load += CapNhatcs_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtTenPhim;
        private Label label2;
        private Label label3;
        private ComboBox cbbTheLoai;
        private Label label4;
        private TextBox txtThoiLuong;
        private Button btnHuy;
        private Button btnLuu;
        private Label label5;
        private TextBox txtGiaTien;
    }
}