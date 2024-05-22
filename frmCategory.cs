using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Go_Mart_Application
{
    public partial class frmCategory : Form
    {
        DBConnect dbcon = new DBConnect();
        public frmCategory()
        {
            InitializeComponent();
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {
            btn_Update.Visible = false;
            btn_Delete.Visible = false;
            lblCatID.Visible = false;
            BindCategory();
        }
        private void btn_AddCategory_Click(object sender, EventArgs e)
        {
            if (txtCatname.Text == String.Empty)
            {   //login code
                MessageBox.Show("Please Enter Category  Name ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCatname.Focus();
                return;

            }
            else if (rtbCatDesc.Text == String.Empty)
            {   //login pass
                MessageBox.Show("Please Enter Category Description", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rtbCatDesc.Focus();
                return;
            }
            else
            {

                SqlCommand cmd = new SqlCommand(" select CategoryName from tblCategory where CategoryName=@CategoryName", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@CategoryName", txtCatname.Text);
                dbcon.OpenCon();
                var result = cmd.ExecuteScalar();
                if (result !=null)
                {
                    MessageBox.Show("CategoryName already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClear();
                }
                else
                {
                    cmd = new SqlCommand("spCatInsert", dbcon.GetCon());
                    cmd.Parameters.AddWithValue("@CategoryName", txtCatname.Text);
                    cmd.Parameters.AddWithValue("@CategoryDesc", rtbCatDesc.Text);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int i = cmd.ExecuteNonQuery();
                    if(i>0)
                    {
                        MessageBox.Show("Category inserted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        BindCategory();
                    }
                }
                dbcon.CloseCon();
                

            }
        }
        private void txtClear()
        {
            txtCatname.Clear();
            rtbCatDesc.Clear();

        }
        private void BindCategory()
        {
            SqlCommand cmd = new SqlCommand("select CatID as CategoryID,CategoryName,CategoryDesc as CategoryDescription from tblCategory", dbcon.GetCon());
            dbcon.OpenCon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dbcon.CloseCon();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            btn_Update.Visible = true;
            btn_Delete.Visible = true;
            lblCatID.Visible = true;
            btn_AddCat.Visible = false;
            lblCatID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtCatname.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            rtbCatDesc.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblCatID.Text == String.Empty)
                {
                    MessageBox.Show("Please select CategoryID ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCatname.Focus();
                    return;

                }
                if (txtCatname.Text == String.Empty)
                {   
                    MessageBox.Show("Please Enter Category  Name ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCatname.Focus();
                    return;

                }
                else if (rtbCatDesc.Text == String.Empty)
                {   
                    MessageBox.Show("Please Enter Category Description", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rtbCatDesc.Focus();
                    return;
                }
                else
                {

                    SqlCommand cmd = new SqlCommand("spCatUpdate", dbcon.GetCon());
                    cmd.Parameters.AddWithValue("@CatID", Convert.ToInt16(lblCatID.Text));
                    cmd.Parameters.AddWithValue("@CategoryName", txtCatname.Text);
                    cmd.Parameters.AddWithValue("@CategoryDesc", rtbCatDesc.Text);
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbcon.OpenCon();
                    int i = cmd.ExecuteNonQuery();
                    dbcon.CloseCon();
                    if (i > 0)
                    {
                        MessageBox.Show("Category Updated Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        BindCategory();
                        btn_Update.Visible = false;
                        btn_Delete.Visible = false;
                        btn_AddCat.Visible = true;
                        lblCatID.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Category Updated Failed...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtClear();
                    }
                   
                }
                 dbcon.CloseCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblCatID.Text == String.Empty)
                {
                    MessageBox.Show("Please select CategoryID ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                if (lblCatID.Text != String.Empty)
                    if(DialogResult.Yes==MessageBox.Show("Do You Want To Delete?","Confirmtion",MessageBoxButtons.YesNo,MessageBoxIcon.Warning))
                    {
                        SqlCommand cmd = new SqlCommand("spCatDelete", dbcon.GetCon());
                        int catID = 0;
                        cmd.Parameters.AddWithValue("@CatID", Convert.ToInt32(lblCatID.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbcon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Category Deleted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindCategory();
                            btn_Update.Visible = false;
                            btn_Delete.Visible = false;
                            btn_AddCat.Visible = true;
                            lblCatID.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show(" Delete Failed...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();
                        }
                        dbcon.CloseCon();
                    }
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }
    }
}
