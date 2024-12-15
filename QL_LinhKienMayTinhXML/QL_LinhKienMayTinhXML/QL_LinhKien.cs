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
    public partial class QL_LinhKien : Form
    {
        SqlConnection connection;
        public QL_LinhKien()
        {
            InitializeComponent();
            connection = DatabaseConnect.GetConnection();
            LoadLoaiLK();
            LoadProducts();

        }



        private void LoadLoaiLK()
        {
            cbxLoaiLK.Items.Clear();

            cbxLoaiLK.Items.Add("RAM");
            cbxLoaiLK.Items.Add("SSD");
            cbxLoaiLK.Items.Add("CPU");
            cbxLoaiLK.Items.Add("Mainboard");
            cbxLoaiLK.Items.Add("GPU");
        }



        private void LoadProducts()
        {
            try
            {
                connection.Open();
                string query = "SELECT a.MaLinhKien, a.TenLinhKien, a.LoaiLinhKien, a.SoLuong, a.HangSanXuat, a.Gia " +
                               "FROM Products a ";


                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvProducts.DataSource = table;

                dgvProducts.Columns[0].HeaderText = "ID";
                dgvProducts.Columns[1].HeaderText = "Tên link kiện";
                dgvProducts.Columns[2].HeaderText = "Loại linh kiện";
                dgvProducts.Columns[3].HeaderText = "Số lượng";
                dgvProducts.Columns[4].HeaderText = "Hãng sản xuất";
                dgvProducts.Columns[5].HeaderText = "Giá";

                dgvProducts.Columns[0].Width = 100;
                dgvProducts.Columns[1].Width = 250;
                dgvProducts.Columns[2].Width = 200;
                dgvProducts.Columns[3].Width = 150;
                dgvProducts.Columns[4].Width = 220;
                dgvProducts.Columns[4].Width = 250;
                dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvProducts.MultiSelect = false;

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

        private void label3_Click(object sender, EventArgs e)
        {

        }

    
        //thêm
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                string query = "INSERT INTO Products (TenLinhKien, LoaiLinhKien, SoLuong, HangSanXuat, Gia) " +
                               "VALUES (@TenLinhKien, @LoaiLinhKien, @SoLuong, @HangSanXuat, @Gia)";

                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@TenLinhKien", textBox11.Text);
                cmd.Parameters.AddWithValue("@LoaiLinhKien", cbxLoaiLK.SelectedItem?.ToString());
                cmd.Parameters.AddWithValue("@SoLuong", int.Parse(textBox1.Text));
                cmd.Parameters.AddWithValue("@HangSanXuat", textBox8.Text);
                cmd.Parameters.AddWithValue("@Gia", decimal.Parse(textBox7.Text));

                int result = cmd.ExecuteNonQuery();
                connection.Close();
                if (result > 0)
                {
                    MessageBox.Show("Thêm thành công!");
                    LoadProducts();
                }
                else
                {
                    MessageBox.Show("Thêm không thành công.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
        //sửa
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                string query = "UPDATE Products SET TenLinhKien = @TenLinhKien, LoaiLinhKien = @LoaiLinhKien, " +
                               "SoLuong = @SoLuong, HangSanXuat = @HangSanXuat, Gia = @Gia " +
                               "WHERE MaLinhKien = @MaLinhKien";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@MaLinhKien", textBox12.Text);
                cmd.Parameters.AddWithValue("@TenLinhKien", textBox11.Text);
                cmd.Parameters.AddWithValue("@LoaiLinhKien", cbxLoaiLK.SelectedItem?.ToString());
                cmd.Parameters.AddWithValue("@SoLuong", int.Parse(textBox1.Text));
                cmd.Parameters.AddWithValue("@HangSanXuat", textBox8.Text);
                cmd.Parameters.AddWithValue("@Gia", decimal.Parse(textBox7.Text));

                int result = cmd.ExecuteNonQuery();
                connection.Close();
                if (result > 0)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    LoadProducts();
                }
                else
                {
                    MessageBox.Show("Cập nhật không thành công.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật dữ liệu: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
        //xóa
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox12.Text == "")
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa.");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    connection.Open();

                    string query = "DELETE FROM Products WHERE MaLinhKien = @MaLinhKien";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@MaLinhKien", textBox12.Text);

                    int result = cmd.ExecuteNonQuery();
                    connection.Close();
                    if (result > 0)
                    {
                        MessageBox.Show("Xóa thành công!");
                        LoadProducts();
                    }
                    else
                    {
                        MessageBox.Show("Xóa không thành công.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa dữ liệu: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                string loaiLinhKien = cbxLoaiLK.SelectedItem?.ToString();
                string query;

                if (!string.IsNullOrEmpty(loaiLinhKien))
                {

                    query = "SELECT a.MaLinhKien, a.TenLinhKien, a.LoaiLinhKien, a.SoLuong, a.HangSanXuat, a.Gia " +
                            "FROM Products a WHERE a.LoaiLinhKien = @LoaiLinhKien";
                }
                else
                {

                    query = "SELECT a.MaLinhKien, a.TenLinhKien, a.LoaiLinhKien, a.SoLuong, a.HangSanXuat, a.Gia " +
                            "FROM Products a";
                }

                SqlCommand cmd = new SqlCommand(query, connection);

                if (!string.IsNullOrEmpty(loaiLinhKien))
                {
                    cmd.Parameters.AddWithValue("@LoaiLinhKien", loaiLinhKien);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dgvProducts.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc dữ liệu: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        //xuất file xml
        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();


            XmlElement root = xmlDoc.CreateElement("Products");
            xmlDoc.AppendChild(root);


            foreach (DataGridViewRow row in dgvProducts.Rows)
            {

                if (row.Cells["MaLinhKien"].Value == null || string.IsNullOrEmpty(row.Cells["MaLinhKien"].Value.ToString()))
                {
                    continue;
                }


                XmlElement productElement = xmlDoc.CreateElement("Product");


                XmlElement maLinhKienElement = xmlDoc.CreateElement("MaLinhKien");
                maLinhKienElement.InnerText = row.Cells["MaLinhKien"].Value.ToString();
                productElement.AppendChild(maLinhKienElement);


                XmlElement tenLinhKienElement = xmlDoc.CreateElement("TenLinhKien");
                tenLinhKienElement.InnerText = row.Cells["TenLinhKien"].Value.ToString();
                productElement.AppendChild(tenLinhKienElement);


                XmlElement loaiLinhKienElement = xmlDoc.CreateElement("LoaiLinhKien");
                loaiLinhKienElement.InnerText = row.Cells["LoaiLinhKien"].Value.ToString();
                productElement.AppendChild(loaiLinhKienElement);


                XmlElement soLuongElement = xmlDoc.CreateElement("SoLuong");
                soLuongElement.InnerText = row.Cells["SoLuong"].Value.ToString();
                productElement.AppendChild(soLuongElement);


                XmlElement hangSanXuatElement = xmlDoc.CreateElement("HangSanXuat");
                hangSanXuatElement.InnerText = row.Cells["HangSanXuat"].Value.ToString();
                productElement.AppendChild(hangSanXuatElement);


                XmlElement giaElement = xmlDoc.CreateElement("Gia");
                giaElement.InnerText = row.Cells["Gia"].Value.ToString();
                productElement.AppendChild(giaElement);


                root.AppendChild(productElement);
            }

            try
            {

                string filePath = "Products.xml";
                xmlDoc.Save(filePath);
                MessageBox.Show("Dữ liệu đã được lưu vào tệp XML thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi ghi vào tệp XML: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cbxLoaiLK_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvProducts_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];

                textBox12.Text = row.Cells["MaLinhKien"].Value.ToString();
                textBox11.Text = row.Cells["TenLinhKien"].Value.ToString();
                textBox1.Text = row.Cells["SoLuong"].Value.ToString();
                textBox8.Text = row.Cells["HangSanXuat"].Value.ToString();
                textBox7.Text = row.Cells["Gia"].Value.ToString();
                string loaiLK = row.Cells["LoaiLinhKien"]?.Value?.ToString();
                if (!string.IsNullOrEmpty(loaiLK) && cbxLoaiLK.Items.Contains(loaiLK))
                {
                    cbxLoaiLK.SelectedItem = loaiLK;
                }
                else
                {
                    cbxLoaiLK.SelectedIndex = -1;
                }

            }
        }
    }
}
