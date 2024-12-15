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
using System.Xml.Serialization;

namespace QL_LinhKienMayTinhXML
{
    public partial class QL_HoaDon : Form
    {
        SqlConnection connection;
        public QL_HoaDon()
        {
            InitializeComponent();
            connection = DatabaseConnect.GetConnection();
            LoadOrders(null);
            LoadMonthCombobox();
            LoadXmlFileNames();

        }

        private void LoadOrders(int? month)
        {
            try
            {
                connection.Open();
                string query;
                SqlCommand cmd;
                SqlDataAdapter adapter;
                DataTable table;
                if (month == null)
                {
                    query = "SELECT o.OrderID, o.CustomerID, c.PhoneNumber, o.OrderDate, o.TotalAmount " +
                        "FROM Orders o " +
                        "INNER JOIN Customers c ON c.CustomerID = o.CustomerID";

                    cmd = new SqlCommand(query, connection);
                }
                else
                {
                    query = "SELECT o.OrderID, o.CustomerID, c.PhoneNumber, o.OrderDate, o.TotalAmount " +
                   "FROM Orders o " +
                   "INNER JOIN Customers c ON c.CustomerID = o.CustomerID " +
                   "WHERE MONTH(o.OrderDate) = @Month";

                    cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Month", month);
                }
                //query = "SELECT o.OrderID, o.CustomerID, c.PhoneNumber, o.OrderDate, o.TotalAmount " +
                //        "FROM Orders o " +
                //        "INNER JOIN Customers c ON c.CustomerID = o.CustomerID";

                //cmd = new SqlCommand(query, connection);
                adapter = new SqlDataAdapter(cmd);
                table = new DataTable();
                adapter.Fill(table);
                dgvOrder.DataSource = table;

                dgvOrder.Columns[0].HeaderText = "ID";
                dgvOrder.Columns[1].HeaderText = "ID khách hàng";
                dgvOrder.Columns[2].HeaderText = "SĐT";
                dgvOrder.Columns[3].HeaderText = "Ngày Mua";
                dgvOrder.Columns[4].HeaderText = "Tổng tiền";


                dgvOrder.Columns[0].Width = 50;
                dgvOrder.Columns[1].Width = 250;
                dgvOrder.Columns[2].Width = 150;
                dgvOrder.Columns[3].Width = 300;
                dgvOrder.Columns[4].Width = 300;

                dgvOrder.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvOrder.MultiSelect = false;




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

        private void LoadXmlFileNames()
        {
            cbxXML.Items.Clear();

           
            cbxXML.Items.Add("OrderAll.xml");

           
            for (int i = 1; i <= 12; i++)
            {
                cbxXML.Items.Add($"Order{i}.xml");
            }

            
            if (cbxXML.Items.Count > 0)
            {
                cbxXML.SelectedIndex = 0;
            }
        }



        private void LoadOrderProduct(string orderId)
        {

            listBox1.Items.Clear();
            string query = "SELECT  oi.Quantity, p.TenLinhKien " +
                   "FROM OrderItem oi " +
                   "INNER JOIN Products p ON oi.MaLinhKien = p.MaLinhKien " +
                   "WHERE oi.OrderID = @OrderID";

            int id;
            if (!Int32.TryParse(orderId, out id))
            {
                MessageBox.Show("Mã đơn hàng không hợp lệ.");
                return;
            }
            try
            {

                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderID", id);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int quantity = reader.GetInt32(0);

                    string productName = reader.GetString(1);


                    string itemText = $"{productName} - Số lượng: {quantity}";
                    listBox1.Items.Add(itemText);
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
        }
        private string GetMonthName(int month)
        {

            string[] monthNames = new string[]
            {
            "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5",
            "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10",
            "Tháng 11", "Tháng 12"

            };

            return monthNames[month - 1];
        }
        private void LoadMonthCombobox()
        {
            cbxMonths.Items.Clear();
            cbxMonths.Items.Add("");

            for (int i = 1; i <= 12; i++)
            {

                cbxMonths.Items.Add(GetMonthName(i));
            }
            cbxMonths.SelectedIndex = 0;
        }

        //Import dữ lieeu từ file xml
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
               
                string selectedFile = cbxXML.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedFile))
                {
                    MessageBox.Show("Vui lòng chọn một tệp XML để nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                string filePath = Path.Combine(Application.StartupPath, selectedFile);

               
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Tệp XML '{selectedFile}' không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                DataTable dataTable = new DataTable("Orders");
                dataTable.Columns.Add("OrderID", typeof(int));
                dataTable.Columns.Add("CustomerID", typeof(string));
                dataTable.Columns.Add("PhoneNumber", typeof(string));
                dataTable.Columns.Add("OrderDate", typeof(DateTime));
                dataTable.Columns.Add("TotalAmount", typeof(decimal));
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(filePath);

                if (dataSet.Tables.Contains("Order"))
                {
                    dataTable = dataSet.Tables["Order"];
                }
                else
                {
                    MessageBox.Show("Dữ liệu trong tệp XML không hợp lệ hoặc trống'.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

               
                dgvOrder.DataSource = dataTable;
                MessageBox.Show($"Dữ liệu đã được nhập thành công từ '{selectedFile}'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi nhập dữ liệu từ tệp XML: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvOrder.Rows[e.RowIndex];


                textBox1.Text = row.Cells["OrderID"].Value.ToString();
                textBox2.Text = row.Cells["CustomerID"].Value.ToString();
                //txtEmail.Text = row.Cells["Email"].Value.ToString();
                textBox5.Text = row.Cells["PhoneNumber"].Value.ToString();
                textBox4.Text = row.Cells["OrderDate"].Value.ToString();
                textBox6.Text = row.Cells["TotalAmount"].Value.ToString();
                string orderId = textBox1.Text;
                //MessageBox.Show("orderId", orderId);
                LoadOrderProduct(orderId);


            }
        }

        //sửa
        private void button2_Click(object sender, EventArgs e)
        {
            string query = "UPDATE Orders SET CustomerID = @CustomerID, TotalAmount = @TotalAmount, OrderDate = @OrderDate  WHERE OrderID = @OrderID";
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);


                cmd.Parameters.AddWithValue("@OrderID", textBox1.Text);
                cmd.Parameters.AddWithValue("@CustomerID", textBox2.Text);
                cmd.Parameters.AddWithValue("@TotalAmount", textBox6.Text);
                DateTime orderDate;
                if (!DateTime.TryParse(textBox4.Text, out orderDate))
                {
                    MessageBox.Show("Ngày đặt hàng không hợp lệ.");
                    return;
                }

                cmd.Parameters.AddWithValue("@OrderDate", orderDate);
                int result = cmd.ExecuteNonQuery();
                connection.Close();
                if (result > 0)
                {
                    MessageBox.Show("Cập nhật đơn hàng thành công!");
                    LoadOrders(null);
                }
                else
                {
                    MessageBox.Show("Cập nhật đơn hàng thất bại!");
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
        //xoa
        private void button3_Click(object sender, EventArgs e)
        {
            string deleteOrderItems = "DELETE FROM OrderItem WHERE OrderID = @OrderID";
            string deleteOrder = "DELETE FROM Orders WHERE OrderID = @OrderID";

            try
            {
                connection.Open();


                SqlCommand cmdDeleteOrderItems = new SqlCommand(deleteOrderItems, connection);
                cmdDeleteOrderItems.Parameters.AddWithValue("@OrderID", textBox1.Text);
                cmdDeleteOrderItems.ExecuteNonQuery();


                SqlCommand cmdDeleteOrder = new SqlCommand(deleteOrder, connection);
                cmdDeleteOrder.Parameters.AddWithValue("@OrderID", textBox1.Text);

                int result = cmdDeleteOrder.ExecuteNonQuery();
                connection.Close();
                if (result > 0)
                {
                    MessageBox.Show("Xóa đơn hàng thành công!");
                    LoadOrders(null);
                }
                else
                {
                    MessageBox.Show("Xóa đơn hàng thất bại!");
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

        private void cbxMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMonth = cbxMonths.SelectedItem.ToString();
            if (selectedMonth == "") return;
            int month = int.Parse(selectedMonth.Replace("Tháng ", "").Trim());
            //MessageBox.Show("month", month.ToString());
            LoadOrders(month);
        }

        //xuất file xml
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                
                int? selectedMonth = null;
                if (cbxMonths.SelectedIndex > 0) 
                {
                    string selectedMonthText = cbxMonths.SelectedItem.ToString();
                    selectedMonth = int.Parse(selectedMonthText.Replace("Tháng ", "").Trim());
                }

               
                string query;
                if (selectedMonth == null)
                {
                    query = "SELECT o.OrderID, o.CustomerID, c.PhoneNumber, o.OrderDate, o.TotalAmount " +
                            "FROM Orders o " +
                            "INNER JOIN Customers c ON c.CustomerID = o.CustomerID";
                }
                else
                {
                    query = "SELECT o.OrderID, o.CustomerID, c.PhoneNumber, o.OrderDate, o.TotalAmount " +
                            "FROM Orders o " +
                            "INNER JOIN Customers c ON c.CustomerID = o.CustomerID " +
                            "WHERE MONTH(o.OrderDate) = @Month";
                }

                DataTable table = new DataTable();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    if (selectedMonth != null)
                    {
                        cmd.Parameters.AddWithValue("@Month", selectedMonth);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(table);
                }

                
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement root = xmlDoc.CreateElement("Orders");
                xmlDoc.AppendChild(root);

                foreach (DataRow row in table.Rows)
                {
                    XmlElement orderElement = xmlDoc.CreateElement("Order");

                    XmlElement orderId = xmlDoc.CreateElement("OrderID");
                    orderId.InnerText = row["OrderID"].ToString();
                    orderElement.AppendChild(orderId);

                    XmlElement customerId = xmlDoc.CreateElement("CustomerID");
                    customerId.InnerText = row["CustomerID"].ToString();
                    orderElement.AppendChild(customerId);

                    XmlElement phoneNumber = xmlDoc.CreateElement("PhoneNumber");
                    phoneNumber.InnerText = row["PhoneNumber"].ToString();
                    orderElement.AppendChild(phoneNumber);

                    XmlElement orderDate = xmlDoc.CreateElement("OrderDate");
                    orderDate.InnerText = DateTime.Parse(row["OrderDate"].ToString()).ToString("yyyy-MM-dd");
                    orderElement.AppendChild(orderDate);

                    XmlElement totalAmount = xmlDoc.CreateElement("TotalAmount");
                    totalAmount.InnerText = row["TotalAmount"].ToString();
                    orderElement.AppendChild(totalAmount);

                    root.AppendChild(orderElement);
                }

               
                string fileName = selectedMonth == null
                    ? "OrderAll.xml"
                    : $"Order{selectedMonth}.xml";

                xmlDoc.Save(fileName);
                MessageBox.Show($"Dữ liệu đã được lưu vào tệp XML thành công: {fileName}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi ghi vào tệp XML: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
