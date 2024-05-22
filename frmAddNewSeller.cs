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

namespace Go_Mart_Application
{
    public partial class frmAddNewSeller : Form
    {
        DBConnect dbcon = new DBConnect();
        public frmAddNewSeller()
        {
            InitializeComponent();
        }

        private void frmAddNewSeller_Load(object sender, EventArgs e)
        {
            lblSellerID.Visible = false;
            btn_Update.Visible = false;
            btn_Delete.Visible = false;
            btn_Add.Visible = true;
            BindSeller();

        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (txtSellerName.Text == String.Empty)
            {   //login code
                MessageBox.Show("Please Enter Seller Name ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSellerName.Focus();
                return;

            }
            else if (txtPass.Text == String.Empty)
            {   //login pass
                MessageBox.Show("Please Enter Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPass.Focus();
                return;
            }
            else
            {

                SqlCommand cmd = new SqlCommand("  select SellerName from tblSeller where SellerName=@SellerName", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                dbcon.OpenCon();
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    MessageBox.Show("SellerName already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClear();
                }
                else
                {
                    cmd = new SqlCommand("spSellerInsert", dbcon.GetCon());
                    cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                    cmd.Parameters.AddWithValue("@SellerAge", Convert.ToInt32(txtAge.Text));
                    cmd.Parameters.AddWithValue("@SellerPhone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@SellerPass", txtPass.Text);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Seller Inserted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        BindSeller();
                    }
                }
                dbcon.CloseCon();


            }
        }

        private void txtClear()
        {
            txtSellerName.Clear();
            txtAge.Clear();
            txtPass.Clear();
            txtPhone.Clear();
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblSellerID.Text == String.Empty)
                {
                    MessageBox.Show("Please select SellerID ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                if (txtSellerName.Text == String.Empty)
                {   
                    MessageBox.Show("Please Enter Name ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSellerName.Focus();
                    return;

                }
                else if (txtPass.Text == String.Empty)
                {   
                    MessageBox.Show("Please Enter Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPass.Focus();
                    return;
                }
                else
                {

                    SqlCommand cmd = new SqlCommand("  select SellerName from tblSeller where SellerName=@SellerName", dbcon.GetCon());
                    cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                    dbcon.OpenCon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Seller Name Already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                    }
                    else
                    {
                        cmd = new SqlCommand("spSellerUpdate", dbcon.GetCon());
                        cmd.Parameters.AddWithValue("@SellerID", Convert.ToInt16(lblSellerID.Text));
                        cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                        cmd.Parameters.AddWithValue("@SellerAge", Convert.ToInt32(txtAge.Text));
                        cmd.Parameters.AddWithValue("@SellerPhone", txtPhone.Text);
                        cmd.Parameters.AddWithValue("@SellerPass", txtPass.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        dbcon.CloseCon();
                        if (i > 0)
                        {
                            MessageBox.Show("Seller Updated Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindSeller();
                            lblSellerID.Visible = false;
                            btn_Update.Visible = false;
                            btn_Delete.Visible = false;
                            btn_Add.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Seller Updated Failed...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();
                        }
                    }
                    dbcon.CloseCon();
                }
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
                if (lblSellerID.Text == String.Empty)
                {
                    MessageBox.Show("Please select SellerID ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                if (lblSellerID.Text != String.Empty)
                {
                    if (DialogResult.Yes == MessageBox.Show("Do You Want To Delete?", "Confirmtion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {

                        SqlCommand cmd = new SqlCommand("spSellerDelete", dbcon.GetCon());
                        cmd.Parameters.AddWithValue("@SellerID", Convert.ToInt16(lblSellerID.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbcon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Seller Deleted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindSeller();
                            btn_Update.Visible = false;
                            btn_Delete.Visible = false;
                            btn_Add.Visible = true;
                            lblSellerID.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show(" Delete Failed...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();
                        }
                        dbcon.CloseCon();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BindSeller()
        {
            SqlCommand cmd = new SqlCommand("select * from tblSeller", dbcon.GetCon());
            dbcon.OpenCon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dbcon.CloseCon();
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                    btn_Update.Visible = true;
                    btn_Delete.Visible = true;
                    btn_Add.Visible = false;
                    lblSellerID.Visible = true;


                    if (selectedRow.Cells.Count >= 5)
                    {
                        lblSellerID.Text = selectedRow.Cells[0].Value?.ToString();
                        txtSellerName.Text = selectedRow.Cells[1].Value?.ToString();
                        txtAge.Text = selectedRow.Cells[2].Value?.ToString();
                        txtPhone.Text = selectedRow.Cells[3].Value?.ToString();
                        txtPass.Text = selectedRow.Cells[4].Value?.ToString();
                    }
                    else
                    {
                        // Handle insufficient cells in the selected row
                        MessageBox.Show("Selected row does not contain enough cells.");
                    }
                }
                else
                {
                    //// Handle no rows selected
                    //MessageBox.Show("No rows selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
