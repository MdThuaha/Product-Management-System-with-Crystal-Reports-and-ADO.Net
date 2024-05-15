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
    public partial class SellerCreate : Form
    {
        public SellerCreate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO sellers (sellername, selleraddress ) VALUES (@n, @a)", con)) 
                {
                    cmd.Parameters.AddWithValue("@n", textBox1.Text);
                    cmd.Parameters.AddWithValue("@a", textBox2.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data saved", "Success");
                    textBox1.Clear();
                    textBox2.Clear();
                    con.Close();
                }
            }
        }
    }
}
