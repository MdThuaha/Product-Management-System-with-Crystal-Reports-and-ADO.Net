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
    public partial class ProductEdit : Form
    {
        DataTable dt;
        public ProductEdit()
        {
            InitializeComponent();
        }

        private void ProductEdit_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;
            LoadData();
        }

        public void LoadData()
        {
            dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Products", con))
                {
                    
                    da.Fill(dt);
                    dt.Columns.Add(new DataColumn("image", typeof(byte[])));
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["image"] = File.ReadAllBytes(Path.Combine(@"..\..\Pictures", dt.Rows[i]["picture"].ToString()));
                    }
                    this.dataGridView1.DataSource = dt;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex== 4)
            {
                int id = int.Parse( dt.Rows[e.RowIndex]["productid"].ToString());
                new ProductUpdate { ProductEdit = this, ProductId = id }.ShowDialog();
            }
            if(e.ColumnIndex== 5)
            {
                int id = int.Parse(dt.Rows[e.RowIndex]["productid"].ToString());
                if(CanSProductDelete(id))
                {
                    using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
                    {
                        using (SqlCommand cmd = new SqlCommand("DELETE  Products WHERE productid=@i", con))
                        {
                            cmd.Parameters.AddWithValue("@i", id);
                            con.Open();
                            cmd.ExecuteNonQuery();

                            con.Close();
                            MessageBox.Show("Product deleted", "Success");
                            this.LoadData();

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Cannot delete product", "Error");
                }
            }
        }
        private bool CanSProductDelete(int id)
        {
            bool ok = true;
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM sales WHERE productid=@i", con))
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
