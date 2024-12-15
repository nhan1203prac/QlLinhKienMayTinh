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
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace QL_LinhKienMayTinhXML
{
    public partial class QL_NhanVien : Form
    {
        SqlConnection connection;
        public QL_NhanVien()
        {
            InitializeComponent();
            connection = DatabaseConnect.GetConnection();
            LoadEmployees();
            LoadAccountId();
        }
        private void LoadEmployees()
        {
            try
            {
                connection.Open();
                string query = "SELECT e.EmployeeID, e.FullName, e.PhoneNumber,e.Email,e.Position,e.Salary,e.AccountID" +
                " FROM Employees e";

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvEmployees.DataSource = table;

                dgvEmployees.Columns[0].HeaderText = "ID";
                dgvEmployees.Columns[1].HeaderText = "Tên Nhân Viên";
                dgvEmployees.Columns[2].HeaderText = "SĐT";
                dgvEmployees.Columns[3].HeaderText = "Email";
                dgvEmployees.Columns[4].HeaderText = "Vị trí làm việc";
                dgvEmployees.Columns[5].HeaderText = "Lương";
                dgvEmployees.Columns[6].HeaderText = "Mã Tài Khoản";

                dgvEmployees.Columns[0].Width = 50;
                dgvEmployees.Columns[1].Width = 150;
                dgvEmployees.Columns[2].Width = 150;
                dgvEmployees.Columns[3].Width = 250;
                dgvEmployees.Columns[4].Width = 200;
                dgvEmployees.Columns[5].Width = 90;
                dgvEmployees.Columns[6].Width = 100;
                dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvEmployees.MultiSelect = false;




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
        private void LoadAccountId()
        {
            try
            {
                connection.Open();
                string query = @"
            SELECT a.AccountID, a.Username
            FROM Accounts a
            WHERE a.AccountID NOT IN (SELECT AccountID FROM Employees)
            AND a.AccountID NOT IN (SELECT AccountID FROM Customers)";

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);


                cbxAccountId.DisplayMember = "AccountID";
                cbxAccountId.ValueMember = "AccountID";
                cbxAccountId.DataSource = table;

                //if (cbxAccountId.Items.Count <= 0)
                //{
                //    cbxAccountId.Items.Clear();
                //    cbxAccountId.Items.Add(""); 
                //    cbxAccountId.SelectedIndex = 0;
                //}
                if (table.Rows.Count == 0)
                {
                    DataRow newRow = table.NewRow();
                    newRow["AccountID"] = DBNull.Value;
                    newRow["Username"] = "";
                    table.Rows.InsertAt(newRow, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải tài khoản: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string fullname = txtTenNV.Text;
                string phone = txtSDT.Text;
                string accountId = cbxAccountId.SelectedValue != null ? cbxAccountId.SelectedValue.ToString() : null;
                string position = txtViTri.Text;
                string salary = txtLuong.Text;
                string email = txtEmail.Text;

                connection.Open();
                string query = "INSERT INTO Employees (AccountID, FullName, Email,PhoneNumber,Position,Salary) VALUES (@AccountID, @FullName, @Email,@PhoneNumber,@Position,@Salary)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@AccountID", Int32.Parse(accountId));
                cmd.Parameters.AddWithValue("@FullName", fullname);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PhoneNumber", phone);
                cmd.Parameters.AddWithValue("@Position", position);
                cmd.Parameters.AddWithValue("@Salary", float.Parse(salary));

                int result = cmd.ExecuteNonQuery();
                connection.Close();
                if (result > 0)
                {
                    MessageBox.Show("Tài khoản đã được thêm thành công!");
                    LoadEmployees();
                    LoadAccountId();
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {

                int employeeId = int.Parse(txtId.Text);
                string fullname = txtTenNV.Text;
                string phone = txtSDT.Text;
                string email = txtEmail.Text;
                string position = txtViTri.Text;
                string salary = txtLuong.Text;
                //string accountId = cbxAccountId.SelectedValue != null ? cbxAccountId.SelectedValue.ToString() : null;

                connection.Open();
                string query = "UPDATE Employees SET FullName = @FullName, Email = @Email, PhoneNumber = @PhoneNumber, Position = @Position, Salary = @Salary WHERE EmployeeID = @EmployeeID";
                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                cmd.Parameters.AddWithValue("@FullName", fullname);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PhoneNumber", phone);
                cmd.Parameters.AddWithValue("@Position", position);
                cmd.Parameters.AddWithValue("@Salary", float.Parse(salary));
                //cmd.Parameters.AddWithValue("@AccountID", accountId != null ? int.Parse(accountId) : (object)DBNull.Value);

                int result = cmd.ExecuteNonQuery();
                connection.Close();

                if (result > 0)
                {
                    MessageBox.Show("Nhân viên đã được cập nhật thành công!");
                    LoadEmployees();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật thông tin nhân viên.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {

                int employeeId = int.Parse(txtId.Text);

                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    connection.Open();
                    string query = "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeId);

                    int result = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (result > 0)
                    {
                        MessageBox.Show("Nhân viên đã được xóa thành công!");
                        LoadEmployees();
                        LoadAccountId();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa nhân viên.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvEmployees.Rows[e.RowIndex];


                txtTenNV.Text = row.Cells["FullName"].Value.ToString();
                txtSDT.Text = row.Cells["PhoneNumber"].Value.ToString();
                //txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtViTri.Text = row.Cells["Position"].Value.ToString();
                txtLuong.Text = row.Cells["Salary"].Value.ToString();
                txtId.Text = row.Cells["EmployeeID"].Value.ToString();
                if (row.Cells["Email"].Value != null)
                {

                    txtEmail.Text = row.Cells["Email"].Value.ToString();
                }
                else
                {
                    txtEmail.Text = "";
                }

            }
        }

        private void btnXML_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();

           
            XmlElement root = xmlDoc.CreateElement("Employees");
            xmlDoc.AppendChild(root);

           
            foreach (DataGridViewRow row in dgvEmployees.Rows)
            {
               
                if (row.IsNewRow) continue;

                
                XmlElement employeeElement = xmlDoc.CreateElement("Employee");

               
                XmlElement idElement = xmlDoc.CreateElement("EmployeeID");
                idElement.InnerText = row.Cells["EmployeeID"].Value != null ? row.Cells["EmployeeID"].Value.ToString() : "Không có ID";
                employeeElement.AppendChild(idElement);

               
                XmlElement fullNameElement = xmlDoc.CreateElement("FullName");
                fullNameElement.InnerText = row.Cells["FullName"].Value != null ? row.Cells["FullName"].Value.ToString() : "Không có tên";
                employeeElement.AppendChild(fullNameElement);

               
                XmlElement phoneElement = xmlDoc.CreateElement("PhoneNumber");
                phoneElement.InnerText = row.Cells["PhoneNumber"].Value != null ? row.Cells["PhoneNumber"].Value.ToString() : "Không có số điện thoại";
                employeeElement.AppendChild(phoneElement);

                
                XmlElement emailElement = xmlDoc.CreateElement("Email");
                emailElement.InnerText = row.Cells["Email"].Value != null ? row.Cells["Email"].Value.ToString() : "Không có email";
                employeeElement.AppendChild(emailElement);

              
                XmlElement positionElement = xmlDoc.CreateElement("Position");
                positionElement.InnerText = row.Cells["Position"].Value != null ? row.Cells["Position"].Value.ToString() : "Không có vị trí";
                employeeElement.AppendChild(positionElement);

               
                XmlElement salaryElement = xmlDoc.CreateElement("Salary");
                salaryElement.InnerText = row.Cells["Salary"].Value != null ? row.Cells["Salary"].Value.ToString() : "0";
                employeeElement.AppendChild(salaryElement);

               
                root.AppendChild(employeeElement);
            }

            try
            {
                string filePath = "Employees.xml"; 
                xmlDoc.Save(filePath);
                MessageBox.Show("Dữ liệu đã được lưu vào tệp XML thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi ghi vào tệp XML: {ex.Message}");
            }
        }
            
        }
    }

