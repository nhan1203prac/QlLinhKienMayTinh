namespace QL_LinhKienMayTinhXML
{
    partial class SignIn
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
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txt_tk = new TextBox();
            txt_mk = new TextBox();
            btn_signin = new Button();
            btn_out = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.hinhsignin;
            pictureBox1.Location = new Point(66, 80);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(360, 429);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(591, 108);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(226, 54);
            label1.TabIndex = 1;
            label1.Text = "Đăng Nhập";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(495, 210);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(134, 25);
            label2.TabIndex = 2;
            label2.Text = "Tên Đăng Nhập";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(495, 272);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(87, 25);
            label3.TabIndex = 3;
            label3.Text = "Mật Khẩu";
            // 
            // txt_tk
            // 
            txt_tk.Location = new Point(672, 210);
            txt_tk.Margin = new Padding(4);
            txt_tk.Name = "txt_tk";
            txt_tk.Size = new Size(249, 31);
            txt_tk.TabIndex = 4;
            // 
            // txt_mk
            // 
            txt_mk.Location = new Point(672, 272);
            txt_mk.Margin = new Padding(4);
            txt_mk.Name = "txt_mk";
            txt_mk.PasswordChar = '*';
            txt_mk.Size = new Size(249, 31);
            txt_mk.TabIndex = 5;
            // 
            // btn_signin
            // 
            btn_signin.BackColor = SystemColors.ControlLightLight;
            btn_signin.BackgroundImage = Properties.Resources.tichDung;
            btn_signin.BackgroundImageLayout = ImageLayout.Zoom;
            btn_signin.ImageAlign = ContentAlignment.MiddleRight;
            btn_signin.Location = new Point(531, 375);
            btn_signin.Margin = new Padding(4);
            btn_signin.Name = "btn_signin";
            btn_signin.RightToLeft = RightToLeft.No;
            btn_signin.Size = new Size(131, 61);
            btn_signin.TabIndex = 6;
            btn_signin.UseVisualStyleBackColor = false;
            btn_signin.Click += btn_signin_Click;
            // 
            // btn_out
            // 
            btn_out.BackColor = SystemColors.ButtonHighlight;
            btn_out.BackgroundImage = Properties.Resources.tichSai;
            btn_out.BackgroundImageLayout = ImageLayout.Zoom;
            btn_out.Location = new Point(765, 375);
            btn_out.Margin = new Padding(4);
            btn_out.Name = "btn_out";
            btn_out.Size = new Size(131, 61);
            btn_out.TabIndex = 7;
            btn_out.UseVisualStyleBackColor = false;
            btn_out.Click += btn_out_Click;
            // 
            // SignIn
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(1000, 562);
            Controls.Add(btn_out);
            Controls.Add(btn_signin);
            Controls.Add(txt_mk);
            Controls.Add(txt_tk);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Margin = new Padding(4);
            Name = "SignIn";
            Text = "SignIn";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txt_tk;
        private TextBox txt_mk;
        private Button btn_signin;
        private Button btn_out;
    }
}
