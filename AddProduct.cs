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
    public partial class AddProduct : Form
    {
        DBConnect dbcon = new DBConnect();
        public AddProduct()
        {
            InitializeComponent();
        }

        private void AddProduct_Load(object sender, EventArgs e)
        {
            BindCategory();
            BindProductList();
            SearchBy_Category();
            btn_Update.Visible = false;
            btn_Delete.Visible = false;
            btn_Add.Visible = true;
            lblProdID.Visible = false;
        }

        private void BindCategory()
        {
            SqlCommand cmd = new SqlCommand("spGetCategory", dbcon.GetCon());
            cmd.CommandType = CommandType.StoredProcedure;
            dbcon.OpenCon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbCategory.DataSource = dt;
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CatId";
            dbcon.CloseCon();
        }
        private void SearchBy_Category()
        {
            SqlCommand cmd = new SqlCommand("spGetCategory", dbcon.GetCon());
            cmd.CommandType = CommandType.StoredProcedure;
            dbcon.OpenCon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbsearch.DataSource = dt;
            cmbsearch.DisplayMember = "CategoryName";
            cmbsearch.ValueMember = "CatId";
            dbcon.CloseCon();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtProdName.Text == String.Empty)
                {   
                    MessageBox.Show("Please Enter Product Name ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProdName.Focus();
                    return;

                }
                else if (Convert.ToInt32(txtPrice.Text) < 0 || txtPrice.Text == String.Empty )
                {   
                    MessageBox.Show("Please Enter Valid Price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrice.Focus();
                    return;
                }
                else if (txtQty.Text == String.Empty || Convert.ToInt32(txtQty.Text) < 0)
                {
                    MessageBox.Show("Please Enter Valid Quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQty.Focus();
                    return;
                }
                else
                {

                    SqlCommand cmd = new SqlCommand("spCheckDuplicateProduct", dbcon.GetCon());
                    cmd.Parameters.AddWithValue("@ProductName", txtProdName.Text);
                    cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbcon.OpenCon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Product Name already Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                    }
                    else
                    {
                        cmd = new SqlCommand("spInsertProduct", dbcon.GetCon());
                        cmd.Parameters.AddWithValue("@ProductName", txtProdName.Text);
                        cmd.Parameters.AddWithValue("@ProdCatId", cmbCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@ProdPrice", Convert.ToDecimal (txtPrice.Text));
                        cmd.Parameters.AddWithValue("@ProdQty",  Convert.ToInt32(txtQty.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Product Inserted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindProductList();
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

        private void BindProductList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetAllProductList", dbcon.GetCon());
                cmd.CommandType = CommandType.StoredProcedure;
                dbcon.OpenCon();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dbcon.CloseCon();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtClear()
        {
            txtProdName.Clear();
            txtPrice.Clear();
            txtQty.Clear();
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {

                if (lblProdID.Text == "" && txtProdName.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter ProductID And Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProdName.Focus();
                    return;

                }
                else if (txtPrice.Text == String.Empty && Convert.ToInt32(txtPrice.Text) >= 0)
                {
                    MessageBox.Show("Please Enter Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrice.Focus();
                    return;
                }
                else if (txtQty.Text == String.Empty && Convert.ToInt32(txtQty.Text) >= 0)
                {
                    MessageBox.Show("Please Enter Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQty.Focus();
                    return;
                }
                else
                {

                    //SqlCommand cmd = new SqlCommand("spCheckDuplicateProduct", dbcon.GetCon());
                    //cmd.Parameters.AddWithValue("@ProductName", txtProdName.Text);
                    //cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //dbcon.OpenCon();
                    //var result = cmd.ExecuteScalar();
                    //if (result != null)
                    //{
                    //    MessageBox.Show("Product Name already Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    txtClear();
                    //}
                    //else
                    //{
                    //}

                    SqlCommand cmd = new SqlCommand("spUpdateProduct", dbcon.GetCon());
                    cmd.Parameters.AddWithValue("@ProductName", txtProdName.Text);
                    cmd.Parameters.AddWithValue("@ProdCatId", cmbCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@ProdPrice", Convert.ToDecimal(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@ProdQty", Convert.ToInt32(txtQty.Text));
                    cmd.Parameters.AddWithValue("@ProductID", Convert.ToInt32(lblProdID.Text));
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbcon.OpenCon();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Product Update Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        BindProductList();
                        lblProdID.Visible = false;
                        btn_Add.Visible = true;
                        btn_Update.Visible = false;
                        btn_Delete.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Update Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    dbcon.CloseCon();
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
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                    btn_Update.Visible = true;
                    btn_Delete.Visible = true;
                    btn_Add.Visible = false;
                    lblProdID.Visible = true;


                    if (selectedRow.Cells.Count >= 5)
                    {
                        lblProdID.Text = selectedRow.Cells[0].Value?.ToString();
                        txtProdName.Text = selectedRow.Cells[1].Value?.ToString();
                        cmbCategory.Text = selectedRow.Cells[2].Value?.ToString();
                        txtPrice.Text = selectedRow.Cells[4].Value?.ToString();
                        txtQty.Text = selectedRow.Cells[5].Value?.ToString();
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

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {

                if (lblProdID.Text == String.Empty)
                {
                    MessageBox.Show("Please select ProductID ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                if (lblProdID.Text != String.Empty)
                {
                    if (DialogResult.Yes == MessageBox.Show("Do You Want To Delete?", "Confirmtion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {

                        SqlCommand cmd = new SqlCommand("spDeleteProduct", dbcon.GetCon());
                        cmd.Parameters.AddWithValue("@ProductID", Convert.ToInt16(lblProdID.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbcon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Product Deleted Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindProductList();
                            btn_Update.Visible = false;
                            btn_Delete.Visible = false;
                            btn_Add.Visible = true;
                            lblProdID.Visible = false;
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void Search_ProductList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetAllProductList_searchByCat", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@ProdCatID", cmbsearch.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                dbcon.OpenCon();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;              
                dbcon.CloseCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search_ProductList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BindProductList();
        }
    }
}
