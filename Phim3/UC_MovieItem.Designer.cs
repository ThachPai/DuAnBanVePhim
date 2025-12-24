namespace Phim3
{
    partial class UC_MovieItem
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
            picPoster = new PictureBox();
            lblTenPhim = new Label();
            lblGia = new Label();
            ((System.ComponentModel.ISupportInitialize)picPoster).BeginInit();
            SuspendLayout();
            // 
            // picPoster
            // 
            picPoster.Location = new Point(41, 39);
            picPoster.Name = "picPoster";
            picPoster.Size = new Size(157, 200);
            picPoster.SizeMode = PictureBoxSizeMode.Zoom;
            picPoster.TabIndex = 0;
            picPoster.TabStop = false;
            // 
            // lblTenPhim
            // 
            lblTenPhim.AutoSize = true;
            lblTenPhim.Font = new Font("Segoe Print", 16.8000011F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTenPhim.ForeColor = Color.FromArgb(255, 128, 0);
            lblTenPhim.Location = new Point(266, 28);
            lblTenPhim.Name = "lblTenPhim";
            lblTenPhim.Size = new Size(108, 51);
            lblTenPhim.TabIndex = 1;
            lblTenPhim.Text = "label1";
            lblTenPhim.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblGia
            // 
            lblGia.AutoSize = true;
            lblGia.Font = new Font("Segoe UI Symbol", 13.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblGia.ForeColor = Color.FromArgb(0, 192, 0);
            lblGia.Location = new Point(266, 208);
            lblGia.Name = "lblGia";
            lblGia.Size = new Size(82, 31);
            lblGia.TabIndex = 1;
            lblGia.Text = "label1";
            lblGia.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // UC_MovieItem
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(lblGia);
            Controls.Add(lblTenPhim);
            Controls.Add(picPoster);
            Name = "UC_MovieItem";
            Size = new Size(1200, 285);
            Load += UC_MovieItem_Load;
            ((System.ComponentModel.ISupportInitialize)picPoster).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picPoster;
        private Label lblTenPhim;
        private Label lblGia;
    }
}
