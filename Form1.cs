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
    public partial class Form1 : Form
    {
        DBConnect dbcon = new DBConnect();
        public static string loginname, logintype, userid;
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbRole.SelectedIndex = 0;
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                if(cmbRole.SelectedIndex>0)
                {
                    if(txtUserName.Text==String.Empty) 
                    {   //login code
                        MessageBox.Show("Please Enter Valid User Name ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtUserName.Focus();
                        return;
                       
                    }
                    if (txtPass.Text == String.Empty )
                    {   //login pass
                        MessageBox.Show("Please Enter Valid Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPass.Focus();
                        return;
                    }
                    if (cmbRole.SelectedIndex > 0 && txtUserName.Text != String.Empty && txtPass.Text != String.Empty)
                    {  //login code

                        if(cmbRole.SelectedIndex==1)
                        {
                            SqlCommand cmd = new SqlCommand("select top 1 AdminID,Password,FullName from tblAdmin where FullName=@FullName and Password=@Password", dbcon.GetCon());
                            cmd.Parameters.AddWithValue("@Fullname",txtUserName.Text);
                            cmd.Parameters.AddWithValue("@Password", txtPass.Text);
                            dbcon.OpenCon();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if(dt.Rows.Count>0)
                            {
                                userid = dt.Rows[0][0].ToString();
                                MessageBox.Show("Login Success Welcome To Home Page", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                loginname = txtUserName.Text;
                                logintype = cmbRole.Text;
                                
                                clrValue();
                                this.Hide();
                                frmMain fm = new frmMain();
                                fm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Login Please Check UserName And Password", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            
                        }
                        else if (cmbRole.Text == "Seller")
                        {//seller code

                            SqlCommand cmd = new SqlCommand("select top 1 SellerID,SellerName,SellerPass from tblSeller where SellerName=@SellerName and SellerPass=@SellerPass", dbcon.GetCon());
                            cmd.Parameters.AddWithValue("@SellerName", txtUserName.Text);
                            cmd.Parameters.AddWithValue("@SellerPass", txtPass.Text);
                            dbcon.OpenCon();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                userid = dt.Rows[0][0].ToString();
                                MessageBox.Show("Login Success Welcome To Home Page", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                loginname = txtUserName.Text;
                                logintype = cmbRole.Text;
                                clrValue();
                                this.Hide();
                                frmMain fm = new frmMain();
                                fm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Login Please Check UserName And Password", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                        }
                    }

                    else
                    {
                        MessageBox.Show("Please Enter User Name or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clrValue();
                    }

                }
                else
                {
                    MessageBox.Show("Please Select Any Role","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    clrValue();
                }
            }
            
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            clrValue();
        }

        private void clrValue()
        {
            cmbRole.SelectedIndex = 0;
            txtUserName.Clear();
            txtPass.Clear();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
