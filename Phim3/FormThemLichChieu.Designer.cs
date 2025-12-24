namespace Phim3
{
    partial class FormThemLichChieu
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
            cbPhim = new ComboBox();
            dtpThoiGian = new DateTimePicker();
            label1 = new Label();
            txtGiaVe = new TextBox();
            textBox2 = new TextBox();
            btnLuu = new Button();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // cbPhim
            // 
            cbPhim.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPhim.FormattingEnabled = true;
            cbPhim.Location = new Point(205, 90);
            cbPhim.Name = "cbPhim";
            cbPhim.Size = new Size(283, 28);
            cbPhim.TabIndex = 0;
            // 
            // dtpThoiGian
            // 
            dtpThoiGian.Format = DateTimePickerFormat.Custom;
            dtpThoiGian.Location = new Point(205, 151);
            dtpThoiGian.Name = "dtpThoiGian";
            dtpThoiGian.Size = new Size(283, 27);
            dtpThoiGian.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(61, 93);
            label1.Name = "label1";
            label1.Size = new Size(81, 20);
            label1.TabIndex = 2;
            label1.Text = "Chọn phim";
            // 
            // txtGiaVe
            // 
            txtGiaVe.Location = new Point(205, 206);
            txtGiaVe.Name = "txtGiaVe";
            txtGiaVe.Size = new Size(281, 27);
            txtGiaVe.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(205, 261);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(281, 27);
            textBox2.TabIndex = 3;
            textBox2.Text = "1";
            // 
            // btnLuu
            // 
            btnLuu.Location = new Point(213, 371);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(94, 29);
            btnLuu.TabIndex = 4;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            btnLuu.Click += btnLuu_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(61, 151);
            label2.Name = "label2";
            label2.Size = new Size(83, 20);
            label2.TabIndex = 2;
            label2.Text = "Ngày chiếu";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(61, 206);
            label3.Name = "label3";
            label3.Size = new Size(35, 20);
            label3.TabIndex = 2;
            label3.Text = "Giá ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(61, 264);
            label4.Name = "label4";
            label4.Size = new Size(51, 20);
            label4.TabIndex = 2;
            label4.Text = "Phòng";
            // 
            // FormThemLichChieu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(524, 503);
            Controls.Add(btnLuu);
            Controls.Add(textBox2);
            Controls.Add(txtGiaVe);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dtpThoiGian);
            Controls.Add(cbPhim);
            Name = "FormThemLichChieu";
            Text = "FormThemLichChieu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cbPhim;
        private DateTimePicker dtpThoiGian;
        private Label label1;
        private TextBox txtGiaVe;
        private TextBox textBox2;
        private Button btnLuu;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}