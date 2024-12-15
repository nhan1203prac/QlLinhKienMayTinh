using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_LinhKienMayTinhXML
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private Form CurrentFormChild;
        private void OpenChildForm(Form chilForm)
        {
            if (CurrentFormChild != null)
            {
                CurrentFormChild.Close();
            }
            CurrentFormChild = chilForm;
            chilForm.TopLevel = false;
            chilForm.FormBorderStyle = FormBorderStyle.None;
            chilForm.Dock = DockStyle.Fill;
            panel_body.Controls.Add(chilForm);
            panel_body.Tag = chilForm;
            chilForm.BringToFront();
            chilForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QL_LinhKien());
            label1.Text = button1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QL_HoaDon());
            label1.Text = button2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QL_NhanVien());
            label1.Text = button3.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QL_TaiKhoan());
            label1.Text = button4.Text;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (CurrentFormChild != null)
            {
                CurrentFormChild.Close();
            }
            label1.Text = "Home";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
