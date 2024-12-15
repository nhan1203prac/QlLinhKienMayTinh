using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace QL_LinhKienMayTinhXML
{
    public partial class QL_TaiKhoan : Form
    {
        SqlConnection connection;
        public QL_TaiKhoan()
        {
            InitializeComponent();
            connection = DatabaseConnect.GetConnection();
            LoadUsers();
            LoadRoles();
        }

        private void LoadRoles()
        {
            try
            {
                connection.Open();
                string query = "SELECT RoleID, RoleName FROM Roles";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                cbxRole.DisplayMember = "RoleName";
                cbxRole.ValueMember = "RoleID";
                cbxRole.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải vai trò: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        private void LoadUsers()
        {
            try
            {
                connection.Open();
                string query = "SELECT a.AccountID, a.Username, a.Password, r.RoleName, a.CreatedAt " +
                               "FROM Accounts a " +
                               "JOIN Roles r ON a.RoleID = r.RoleID";

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvUsers.DataSource = table;

                dgvUsers.Columns[0].HeaderText = "ID";
                dgvUsers.Columns[1].HeaderText = "Tài Khoản";
                dgvUsers.Columns[2].HeaderText = "Mật Khẩu";
                dgvUsers.Columns[3].HeaderText = "Vai Trò";
                dgvUsers.Columns[4].HeaderText = "Ngày Tạo";

                dgvUsers.Columns[0].Width = 150;
                dgvUsers.Columns[1].Width = 200;
                dgvUsers.Columns[2].Width = 200;
                dgvUsers.Columns[3].Width = 150;
                dgvUsers.Columns[4].Width = 300;
                dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvUsers.MultiSelect = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        private int GetRoleID(string roleName)
        {
            int roleID = 0;
            try
            {
                //Console.WriteLine($"Role Name: {roleName}");
                connection.Open();
                string query = "SELECT RoleID FROM Roles WHERE RoleName = @RoleName";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@RoleName", roleName);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    roleID = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy RoleID: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return roleID;
        }


       //Thêm tài khoản

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtTK.Text;
                string password = txtPassword.Text;
                string roleName = cbxRole.SelectedItem != null ? ((DataRowView)cbxRole.SelectedItem)["RoleName"].ToString() : null;

                int roleID = GetRoleID(roleName);  

                connection.Open();
                string query = "INSERT INTO Accounts (Username, Password, RoleID) VALUES (@Username, @Password, @RoleID)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@RoleID", roleID); 

                int result = cmd.ExecuteNonQuery();
                connection.Close();
                if (result > 0)
                {
                    MessageBox.Show("Tài khoản đã được thêm thành công!");
                    LoadUsers(); 
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi thêm tài khoản.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm tài khoản: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
        //xuất file xml
        private void button4_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();

            
            XmlElement root = xmlDoc.CreateElement("Users");
            xmlDoc.AppendChild(root);

            
            foreach (DataGridViewRow row in dgvUsers.Rows)
            {


                if (row.Cells["AccountID"].Value == null || string.IsNullOrEmpty(row.Cells["AccountID"].Value.ToString()))
                {
                    continue;
                }

                XmlElement userElement = xmlDoc.CreateElement("User");

               
                XmlElement accountIDElement = xmlDoc.CreateElement("AccountID");
                accountIDElement.InnerText = row.Cells["AccountID"]?.Value.ToString();
                userElement.AppendChild(accountIDElement);

                XmlElement usernameElement = xmlDoc.CreateElement("Username");
                usernameElement.InnerText = row.Cells["Username"]?.Value.ToString();
                userElement.AppendChild(usernameElement);

                XmlElement passwordElement = xmlDoc.CreateElement("Password");
                passwordElement.InnerText = row.Cells["Password"]?.Value.ToString();
                userElement.AppendChild(passwordElement);

                XmlElement roleElement = xmlDoc.CreateElement("RoleName");
                roleElement.InnerText = row.Cells["RoleName"]?.Value.ToString();
                userElement.AppendChild(roleElement);

                XmlElement createdAtElement = xmlDoc.CreateElement("CreatedAt");
                createdAtElement.InnerText = row.Cells["CreatedAt"]?.Value.ToString();
                userElement.AppendChild(createdAtElement);

               
                root.AppendChild(userElement);
            }

           
            try
            {
                string filePath = "Users.xml"; 
                xmlDoc.Save(filePath);
                MessageBox.Show("Dữ liệu đã được lưu vào tệp XML thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi ghi vào tệp XML: {ex.Message}");
            }
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];


                txtMTK.Text = row.Cells["AccountID"].Value.ToString();
                txtTK.Text = row.Cells["Username"].Value.ToString();
                txtPassword.Text = row.Cells["Password"].Value.ToString();
               
                txtCreatedAt.Text = row.Cells["CreatedAt"].Value.ToString();
                if (row.Cells["RoleName"] != null)
                {
                    string roleName = row.Cells["RoleName"].Value.ToString();
                    DataTable roleTable = (DataTable)cbxRole.DataSource;

                    DataRow[] selectedRows = roleTable.Select($"RoleName = '{roleName}'");
                    if (selectedRows.Length > 0)
                    {
                        cbxRole.SelectedValue = selectedRows[0]["RoleID"];
                    }
                    else
                    {
                        cbxRole.SelectedIndex = -1;
                    }
                }
            }
        }
        //Update tài khoản
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int accountID = Convert.ToInt32(txtMTK.Text);
                string username = txtTK.Text;
                string password = txtPassword.Text;
                string roleName = cbxRole.SelectedItem != null ? ((DataRowView)cbxRole.SelectedItem)["RoleName"].ToString() : null;

                int roleID = GetRoleID(roleName);
                //MessageBox.Show("role", roleID.ToString());
                if (roleID == 0)
                {
                    MessageBox.Show("Vai trò không hợp lệ. Vui lòng chọn vai trò khác.");
                    return;
                }
                connection.Open();
                string query = "UPDATE Accounts SET Username = @Username, Password = @Password, RoleID = @RoleID WHERE AccountID = @AccountID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@RoleID", roleID);  
                cmd.Parameters.AddWithValue("@AccountID", accountID);

                int result = cmd.ExecuteNonQuery();
                connection.Close();
                if (result > 0)
                {
                    MessageBox.Show("Tài khoản đã được cập nhật!");
                    LoadUsers(); 
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi cập nhật tài khoản.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật tài khoản: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
       // Xóa tài khoản
        private void button3_Click(object sender, EventArgs e)
        {
            if (txtMTK.Text == "")
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa.");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    int accountId = int.Parse(txtMTK.Text);
                    string roleName = cbxRole.SelectedItem != null ? cbxRole.SelectedItem.ToString() : string.Empty;

                    
                    if (roleName == "Nhân viên")
                    {
                        
                        string deleteEmployeeQuery = "DELETE FROM Employees WHERE AccountID = @AccountID";
                        using (SqlCommand cmdEmployee = new SqlCommand(deleteEmployeeQuery, connection))
                        {
                            cmdEmployee.Parameters.AddWithValue("@AccountID", accountId);
                            connection.Open();
                            cmdEmployee.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                   
                    else if (roleName == "Khách hàng")
                    {
                        
                        string deleteCustomerQuery = "DELETE FROM Customers WHERE AccountID = @AccountID";
                        using (SqlCommand cmdCustomer = new SqlCommand(deleteCustomerQuery, connection))
                        {
                            cmdCustomer.Parameters.AddWithValue("@AccountID", accountId);
                            connection.Open();
                            cmdCustomer.ExecuteNonQuery();
                            connection.Close();
                        }
                    }

                   
                    string query = "DELETE FROM Accounts WHERE AccountID = @AccountID";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@AccountID", accountId);
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        connection.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Tài khoản và dữ liệu liên quan đã được xóa thành công.");
                            LoadUsers();
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy tài khoản cần xóa.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa tài khoản: {ex.Message}");
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }



        private void ClearFields()
        {
            txtMTK.Clear();         
            txtTK.Clear();          
            txtPassword.Clear();    
            cbxRole.SelectedIndex = -1;  
            txtCreatedAt.Clear();   
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

    }


}
