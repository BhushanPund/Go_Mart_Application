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
    public partial class AddAdmin : Form
    {
        DBConnect dbcon = new DBConnect();
        public AddAdmin()
        {
            InitializeComponent();

        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAdminName.Text == String.Empty || txtAdminID.Text == String.Empty || txtPass.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Valid AdminID Admin Name And Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clrbtn();
                }
                else
                {//check duplicate records

                    SqlCommand cmd = new SqlCommand("  select AdminID from tblAdmin where AdminID=@ID", dbcon.GetCon());
                    cmd.Parameters.AddWithValue("@ID", txtAdminID.Text);
                    dbcon.OpenCon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("AdminID Name Already Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        clrbtn();
                    }
                    else
                    {
                        cmd = new SqlCommand("spAddAdmin", dbcon.GetCon());
                        cmd.Parameters.AddWithValue("@AdminID", txtAdminID.Text);
                        cmd.Parameters.AddWithValue("@Password", txtPass.Text);
                        cmd.Parameters.AddWithValue("@FullName", txtAdminName.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Admin Inserted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clrbtn();
                            BindAdmin();
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

        private void BindAdmin()
        {
            SqlCommand cmd = new SqlCommand("select * from tblAdmin", dbcon.GetCon());
            dbcon.OpenCon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dbcon.CloseCon();

        }


        private void AddAdmin_Load(object sender, EventArgs e)
        {
            lblAdminID.Visible = false;
            btn_Update.Visible = false;
            btn_Delete.Visible = false;
            btn_Add.Visible = true;
            txtAdminName.Focus();
            BindAdmin();
        }

        public void clrbtn()
        {
            txtAdminName.Clear();
            txtAdminID.Clear();
            txtPass.Clear();
            txtAdminName.Focus();
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAdminName.Text == String.Empty || txtAdminID.Text == String.Empty || txtPass.Text == String.Empty || lblAdminID.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Valid AdminID Admin Name And Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clrbtn();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("spUpdateAdmin", dbcon.GetCon());
                    dbcon.OpenCon();
                    cmd.Parameters.AddWithValue("@AdminID", lblAdminID.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPass.Text);
                    cmd.Parameters.AddWithValue("@FullName", txtAdminName.Text);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        
                        MessageBox.Show("Admin Record Update Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clrbtn();
                        BindAdmin();
                        btn_Update.Visible = false;
                        btn_Delete.Visible = false;
                        btn_Add.Visible = true;
                        lblAdminID.Visible = false;


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
                if (lblAdminID.Text == String.Empty)
                {
                    MessageBox.Show("Please select Admin Record", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (DialogResult.Yes == MessageBox.Show("Do You Want To Delete?", "Confirmtion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {

                        SqlCommand cmd = new SqlCommand("spDeleteAdmin", dbcon.GetCon());
                        cmd.Parameters.AddWithValue("@AdminID", lblAdminID.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbcon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Admin Deleted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clrbtn();
                            BindAdmin();
                            btn_Update.Visible = false;
                            btn_Delete.Visible = false;
                            btn_Add.Visible = true;
                            lblAdminID.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show(" Delete Failed...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            clrbtn();
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


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Make sure the clicked cell is within a row
                if (e.RowIndex >= 0)
                {
                    // Get the selected row
                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                    btn_Update.Visible = true;
                    btn_Delete.Visible = true;
                    btn_Add.Visible = false;
                    lblAdminID.Visible = true;
                    // Ensure the selected row has enough cells
                    if (selectedRow.Cells.Count >= 3) // Assuming you have at least 3 columns
                    {
                        // Display data in text boxes
                        lblAdminID.Text = selectedRow.Cells[0].Value?.ToString();
                        txtAdminName.Text = selectedRow.Cells[2].Value?.ToString();
                        txtPass.Text = selectedRow.Cells[1].Value?.ToString();
                        txtAdminID.Text = selectedRow.Cells[0].Value.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Selected row does not contain enough cells.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
