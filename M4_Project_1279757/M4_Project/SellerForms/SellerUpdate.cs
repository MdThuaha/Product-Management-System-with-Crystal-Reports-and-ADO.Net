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

namespace M4_Project.SellerForms
{
    public partial class SellerUpdate : Form
    {
        public SellerUpdate()
        {
            InitializeComponent();
        }
        public int SellerId { get; set; }
        public SellerEdit SellerEdit { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE Sellers SET sellername=@n, selleraddress=@a WHERE sellerid=@i", con))
                {
                    cmd.Parameters.AddWithValue("@n", textBox1.Text);
                    cmd.Parameters.AddWithValue("@a", textBox2.Text);
                    cmd.Parameters.AddWithValue("@i", this.SellerId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Data saved", "Success");
                    this.SellerEdit.LoadData();
                    

                }
            }
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Sellers WHERE sellerid=@i", con))
                {
                   
                    cmd.Parameters.AddWithValue("@i", this.SellerId);
                    con.Open();
                    SqlDataReader dr= cmd.ExecuteReader();
                    if(dr.Read())
                    {
                        textBox1.Text = dr["sellername"].ToString();
                        textBox2.Text = dr["selleraddress"].ToString();
                    }
                    
                    con.Close();
                   
                }
            }
        }

        private void SellerUpdate_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
