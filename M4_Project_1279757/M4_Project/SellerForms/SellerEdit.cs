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
using System.Windows.Forms.VisualStyles;

namespace M4_Project.SellerForms
{
    public partial class SellerEdit : Form
    {
        DataTable dt;
        public SellerEdit()
        {
            InitializeComponent();
        }

        private void SellerEdit_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;
            LoadData();
        }

        public void LoadData()
        {
            dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Sellers", con))
                {
                    da.Fill(dt);
                   this.dataGridView1.DataSource = dt;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 3) { 
                int id = int.Parse(dt.Rows[e.RowIndex][0].ToString());
                new SellerUpdate { SellerEdit = this, SellerId = id }.ShowDialog();
            }
            if(e.ColumnIndex == 4) {
                int id = int.Parse(dt.Rows[e.RowIndex][0].ToString());
                if(CanSellerDelete(id))
                {
                    using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
                    {
                        using (SqlCommand cmd = new SqlCommand("DELETE  Sellers WHERE sellerid=@i", con))
                        {
                            cmd.Parameters.AddWithValue("@i", id);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            
                            con.Close();
                            MessageBox.Show("Seller deleted");
                            this.LoadData();

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Cannot delete seller", "Error");
                }
            }
        }
        private bool CanSellerDelete(int id)
        {
            bool ok = true;
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using(SqlCommand cmd = new SqlCommand("SELECT 1 FROM Sales WHERE sellerid=@i", con))
                {
                    cmd.Parameters.AddWithValue("@i", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        ok = false;
                    }
                    con.Close();
                }
            }
            return ok;
        }
    }
}
