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

namespace M4_Project.ProductForms
{
    public partial class CreatProduct : Form
    {
        string currentPic = "";
        List<Product> products = new List<Product>();
        public CreatProduct()
        {
            InitializeComponent();
        }
        public MainForm MainForm { get; set; }
        private void CreatProduct_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           if(this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = Image.FromFile(this.openFileDialog1.FileName);
                this.currentPic = this.openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            products.Add(new Product {productid=products.Count+1, productname = textBox1.Text, price = numericUpDown1.Value, picture = currentPic });
            currentPic = "";
            textBox1.Clear();
            pictureBox1.Image = null;
            numericUpDown1.Value = 0;
            this.dataGridView1.DataSource = null;
            this.dataGridView1 .DataSource = products;
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                con.Open();
                using(SqlTransaction trx = con.BeginTransaction())
                {
                    try
                    {
                        foreach (Product product in products)
                        {
                            SqlCommand cmd = new SqlCommand("INSERT INTO Products (productname, unitprice, picture) VALUES(@n, @u, @p)", con, trx);
                            cmd.Parameters.AddWithValue("@n", product.productname);
                            cmd.Parameters.AddWithValue("@u", product.price);
                            string ext = Path.GetExtension(product.picture);
                            string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())+ext;
                            string savePath = Path.Combine(@"..\..\Pictures", fileName);
                            File.Copy(product.picture, savePath);
                            cmd.Parameters.AddWithValue("@p", fileName);
                            cmd.ExecuteNonQuery();
                           
                        }
                        trx.Commit();
                        MessageBox.Show("All data saved.", "Success");
                        products.Clear();
                        this.dataGridView1.DataSource = null;
                        this.dataGridView1.DataSource = products;
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 4)
            {
              int id= int.Parse( dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim());
              int index =  products.FindIndex(x => x.productid == id);
                if(index >= 0)
                {
                    products.RemoveAt(index);
                    this.dataGridView1.DataSource = null;
                    this.dataGridView1.DataSource = products;
                }
            }
        }

        private void CreatProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.MainForm.LoadData();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
