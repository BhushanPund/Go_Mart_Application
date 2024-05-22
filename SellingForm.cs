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
    public partial class SellingForm : Form
    {
        DBConnect dbcon = new DBConnect();
        public SellingForm()
        {
            InitializeComponent();
        }
        Double GrandTotal = 0.0;
        int n = 0;

        private void SellingForm_Load(object sender, EventArgs e)
        {
            BindCategory();
            lblDate1.Text = DateTime.Now.ToShortDateString();
            BindBillList();
        }
        private void Search_ProductList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetAllProductList_searchByCat", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                dbcon.OpenCon();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2_Product.DataSource = dt;
                dbcon.CloseCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BindCategory()
        {
            try
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Search_ProductList();
        }

        private void dataGridView2_Product_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dataGridView2_Product.Rows[e.RowIndex];

                    //btn_Update.Visible = true;
                    //btn_Delete.Visible = true;
                    //btn_Add.Visible = false;
                    //lblProdID.Visible = true;


                    if (selectedRow.Cells.Count >= 5)
                    {
                        txtProdID.Clear();
                        txtProdID.Text = selectedRow.Cells[0].Value?.ToString();
                        txtProdName.Clear();
                        txtProdName.Text = selectedRow.Cells[1].Value?.ToString();
                        //cmbCategory.Text = selectedRow.Cells[2].Value?.ToString();
                        txtPrice.Clear();
                        txtPrice.Text = selectedRow.Cells[4].Value?.ToString();
                        //txtQty.Text = selectedRow.Cells[3].Value?.ToString();
                        txtQty.Clear();
                        txtQty.Focus();
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

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtPrice.Text==""|| txtQty.Text=="")
                {
                    MessageBox.Show("Enter Valid Quantity And Price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    double Total = Convert.ToDouble(txtPrice.Text) * Convert.ToInt32(txtQty.Text);
                    DataGridViewRow addrow = new DataGridViewRow();
                    addrow.CreateCells(dataGridView1_Order);
                    addrow.Cells[0].Value = ++n;
                    addrow.Cells[1].Value = txtProdName.Text;
                    addrow.Cells[2].Value = txtPrice.Text;
                    addrow.Cells[3].Value = txtQty.Text;
                    addrow.Cells[4].Value = Total;
                    dataGridView1_Order.Rows.Add(addrow);
                    GrandTotal += Total;
                    lblGrandTot.Text = GrandTotal.ToString();
                }
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddBill_Details_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtBillNo.Text=="")
                {
                    MessageBox.Show("Enter Bill Number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if(GrandTotal == 0.0)
                {
                    MessageBox.Show("Please select product !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    SqlCommand cmd = new SqlCommand("spInsertBill", dbcon.GetCon());
                    cmd.Parameters.AddWithValue("@Bill_ID", Convert.ToInt32(txtBillNo.Text));
                    cmd.Parameters.AddWithValue("@SellerID", Form1.userid);
                    cmd.Parameters.AddWithValue("@SellDate", lblDate1.Text);
                    cmd.Parameters.AddWithValue("@TotalAmt", Convert.ToDouble(lblGrandTot.Text));
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbcon.OpenCon();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        BindBillList();
                        MessageBox.Show("Bill Added Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     
                    }
                    dbcon.CloseCon();
                    clrtext();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BindBillList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetBillList", dbcon.GetCon());
                cmd.CommandType = CommandType.StoredProcedure;
                dbcon.OpenCon();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView3_Sales.DataSource = dt;
                dbcon.CloseCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void clrtext()
        {
            txtBillNo.Clear();
            dataGridView1_Order.DataSource = null;
            txtPrice.Clear();
            txtProdID.Clear();
            txtProdName.Clear();
            txtQty.Clear();
            lblGrandTot.Text = "0.0";
        }

        private void btnRefCat_Click(object sender, EventArgs e)
        {

            BindProductList();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Search_ProductList();
            try
            {
                SqlCommand cmd = new SqlCommand("spGetAllProductList_searchByCat", dbcon.GetCon());
                cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                dbcon.OpenCon();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2_Product.DataSource = dt;
                dbcon.CloseCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
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
                dataGridView2_Product.DataSource = dt;
                dbcon.CloseCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


