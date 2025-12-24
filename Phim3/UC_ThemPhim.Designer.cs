namespace Phim3
{
    partial class UC_ThemPhim
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
            label1 = new Label();
            txtTitle = new TextBox();
            label2 = new Label();
            txtGenre = new TextBox();
            label3 = new Label();
            label4 = new Label();
            txtDuration = new TextBox();
            txtPrice = new TextBox();
            label5 = new Label();
            label6 = new Label();
            txtPosterUrl = new TextBox();
            txtDescription = new TextBox();
            btnThem = new Button();
            label7 = new Label();
            dtpNgayChieu = new DateTimePicker();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(46, 80);
            label1.Name = "label1";
            label1.Size = new Size(70, 20);
            label1.TabIndex = 0;
            label1.Text = "Tên phim";
            label1.Click += label1_Click;
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(291, 80);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(219, 27);
            txtTitle.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(46, 129);
            label2.Name = "label2";
            label2.Size = new Size(62, 20);
            label2.TabIndex = 0;
            label2.Text = "Thể loại";
            label2.Click += label2_Click;
            // 
            // txtGenre
            // 
            txtGenre.Location = new Point(291, 129);
            txtGenre.Name = "txtGenre";
            txtGenre.Size = new Size(219, 27);
            txtGenre.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(46, 174);
            label3.Name = "label3";
            label3.Size = new Size(81, 20);
            label3.TabIndex = 0;
            label3.Text = "Thời lượng";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(46, 223);
            label4.Name = "label4";
            label4.Size = new Size(50, 20);
            label4.TabIndex = 0;
            label4.Text = "Giá vé";
            // 
            // txtDuration
            // 
            txtDuration.Location = new Point(291, 174);
            txtDuration.Name = "txtDuration";
            txtDuration.Size = new Size(219, 27);
            txtDuration.TabIndex = 1;
            // 
            // txtPrice
            // 
            txtPrice.Location = new Point(291, 223);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(219, 27);
            txtPrice.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(46, 276);
            label5.Name = "label5";
            label5.Size = new Size(63, 20);
            label5.TabIndex = 0;
            label5.Text = "Link ảnh";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(46, 325);
            label6.Name = "label6";
            label6.Size = new Size(48, 20);
            label6.TabIndex = 0;
            label6.Text = "Mô tả";
            // 
            // txtPosterUrl
            // 
            txtPosterUrl.Location = new Point(291, 276);
            txtPosterUrl.Multiline = true;
            txtPosterUrl.Name = "txtPosterUrl";
            txtPosterUrl.Size = new Size(219, 27);
            txtPosterUrl.TabIndex = 1;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(291, 325);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(219, 27);
            txtDescription.TabIndex = 1;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(213, 469);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 2;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(46, 374);
            label7.Name = "label7";
            label7.Size = new Size(83, 20);
            label7.TabIndex = 0;
            label7.Text = "Ngày chiếu";
            // 
            // dtpNgayChieu
            // 
            dtpNgayChieu.Location = new Point(291, 389);
            dtpNgayChieu.Name = "dtpNgayChieu";
            dtpNgayChieu.Size = new Size(250, 27);
            dtpNgayChieu.TabIndex = 3;
            // 
            // UC_ThemPhim
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dtpNgayChieu);
            Controls.Add(btnThem);
            Controls.Add(txtDescription);
            Controls.Add(txtPrice);
            Controls.Add(txtPosterUrl);
            Controls.Add(txtDuration);
            Controls.Add(txtGenre);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(txtTitle);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "UC_ThemPhim";
            Size = new Size(574, 532);
            Load += UC_ThemPhim_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtTitle;
        private Label label2;
        private TextBox txtGenre;
        private Label label3;
        private Label label4;
        private TextBox txtDuration;
        private TextBox txtPrice;
        private Label label5;
        private Label label6;
        private TextBox txtPosterUrl;
        private TextBox txtDescription;
        private Button btnThem;
        private Label label7;
        private DateTimePicker dtpNgayChieu;
    }
}
