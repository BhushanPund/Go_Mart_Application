using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go_Mart_Application
{
    internal class DBConnect
    {
        private SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-FTV41EC;Initial Catalog=GoMartDB;Integrated Security=True");
        public SqlConnection GetCon()
        {
            return con;
        }
        public void OpenCon()
        {

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
        }
        public void CloseCon()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}