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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace M4_Project.ProductForms
{
    public partial class ProductUpdate : Form
    {
        string currentPic = "";
        string fileName = "";
        public ProductUpdate()
        {
            InitializeComponent();
        }
        public int ProductId {  get; set; }
        public ProductEdit ProductEdit { get; set; }
        private void ProductUpdate_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE productid=@i", con))
                {

                    cmd.Parameters.AddWithValue("@i", this.ProductId);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox1.Text = dr["productname"].ToString();
                        numericUpDown1.Value = decimal.Parse(dr["unitprice"].ToString());
                        pictureBox1.Image = Image.FromFile(Path.Combine(@"..\..\Pictures", dr["picture"].ToString()));
                        currentPic = dr["picture"].ToString();
                    }

                    con.Close();

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = Image.FromFile(this.openFileDialog1.FileName);
                this.fileName = this.openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE Products set productname=@n, unitprice=@p, picture=@pic WHERE productid=@i", con))
                {

                    cmd.Parameters.AddWithValue("@i", this.ProductId);
                    cmd.Parameters.AddWithValue("@n", textBox1.Text);
                    cmd.Parameters.AddWithValue("@p", numericUpDown1.Value);
                    if(fileName != "")
                    {
                        string ext = Path.GetExtension(this.fileName);
                        string f= Path.GetFileNameWithoutExtension(Path.GetRandomFileName())+ext;
                        string savePath = Path.Combine(@"..\..\Pictures", f);
                        File.Copy(this.fileName, savePath);
                        currentPic = f;
                        cmd.Parameters.AddWithValue("@pic", f);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@pic", currentPic);
                    }
                    con.Open();
                    cmd.ExecuteNonQuery();
                   
                    con.Close();
                    MessageBox.Show("Data updated", "Success");
                    this.ProductEdit.LoadData();

                }
            }
        }
    }
}
