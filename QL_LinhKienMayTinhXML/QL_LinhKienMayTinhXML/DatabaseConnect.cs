using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_LinhKienMayTinhXML
{
    public class DatabaseConnect
    {
        private static string connectionString = @"Data Source=DESKTOP-U2GTCVE;Initial Catalog=LinhKienMayTinh;Persist Security Info=True;User ID=sa;Password=123456789;Encrypt=False";

        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                return connection;
            }
            
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
                throw;
            }
        }
    }
}
