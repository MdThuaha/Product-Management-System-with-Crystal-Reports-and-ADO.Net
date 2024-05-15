using M4_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M4_Project
{
    public partial class MasterDetail : Form
    {
        List<Sale> sales = new List<Sale>();
       string currentPic = string.Empty;
        public MasterDetail()
        {
            InitializeComponent();
        }

        private void MasterDetail_Load(object sender, EventArgs e)
        {
            LoadCombo();
        }
        private void LoadCombo()
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM sellers", con))
                {
                    
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);
                    
                    comboBox2.DataSource = dt2;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sales.Add(new Sale { date=dateTimePicker1.Value,  sellerid=int.Parse(comboBox2.SelectedValue.ToString()), quantity=(int)numericUpDown2.Value});
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = sales;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                con.Open();
                using (SqlTransaction trx = con.BeginTransaction())
                {
                    try
                    {
                        
                       SqlCommand cmd = new SqlCommand("INSERT INTO Products (productname, unitprice, picture) VALUES(@n, @u, @p)", con, trx);
                       cmd.Parameters.AddWithValue("@n", textBox1.Text);
                       cmd.Parameters.AddWithValue("@u",numericUpDown1.Value);
                        string ext = Path.GetExtension(currentPic);
                        string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string savePath = Path.Combine(@"..\..\Pictures", fileName);
                        File.Copy(currentPic, savePath);
                        cmd.Parameters.AddWithValue("@p", fileName);
                        cmd.ExecuteNonQuery();
                        SqlCommand cmd0 = new SqlCommand("SELECT IDENT_CURRENT('Products')", con, trx);
                        int pid = -1;
                        var dr = cmd0.ExecuteReader();
                        if(dr.Read())
                        {
                            pid = int.Parse(dr[0].ToString());
                        }
                        else
                        {
                            throw new Exception("Cannot get new productid");
                        }
                        dr.Close();
                      foreach(Sale sale in sales) {
                            SqlCommand cmd1 = new SqlCommand("INSERT INTO sales ([date], sellerid, productid, quantity) VALUES (@d1, @s1, @p1, @q1)", con, trx);
                            cmd1.Parameters.AddWithValue("@d1",sale.date);
                            cmd1.Parameters.AddWithValue("@s1", sale.sellerid);
                            cmd1.Parameters.AddWithValue("@p1", pid);
                            cmd1.Parameters.AddWithValue("@q1", sale.quantity);
                            cmd1.ExecuteNonQuery();
                        }
                        trx.Commit();
                        MessageBox.Show($"Data saved", "Success");
                        textBox1.Clear();
                        numericUpDown1.Value = 0;
                        numericUpDown2.Value = 0;
                        sales.Clear();
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = sales;
                    }
                    catch (Exception ex)
                    {
                        trx.Rollback();
                        MessageBox.Show($"Failed to save data: {ex.Message}", "Failed");
                    }
                }
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = Image.FromFile(this.openFileDialog1.FileName);
                this.currentPic = this.openFileDialog1.FileName;
            }
        }
    }
}
