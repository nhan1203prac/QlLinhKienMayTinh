using System.Data.SqlClient;

namespace QL_LinhKienMayTinhXML
{
    public partial class SignIn : Form
    {
        public SignIn()
        {
            InitializeComponent();
            using (SqlConnection con = DatabaseConnect.GetConnection())
            {


            }
        }

        private void btn_signin_Click(object sender, EventArgs e)
        {
            if (CheckLogin(txt_tk.Text, txt_mk.Text))
            {
                MessageBox.Show("Đăng nhập thành công");
                Main mainForm = new Main();
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Tài Khoản hoặc mật khẩu không chính xác!!");
            }

        }

        public bool CheckLogin(string username, string password)
        {
            using (SqlConnection conn = DatabaseConnect.GetConnection())
            {
                conn.Open();
                string query = "Select COUNT(*) from Accounts where Username = @Username and Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                int count = (int)cmd.ExecuteScalar();
                conn.Close();
                return count > 0;
            }
        }

        private void btn_out_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
