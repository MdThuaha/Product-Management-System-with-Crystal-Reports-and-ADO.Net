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

namespace M4_Project.SalesForms
{
    public partial class SalesEdit : Form
    {
        public SalesEdit()
        {
            InitializeComponent();
        }

        private void SalesEdit_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            LoadData();
        }
        public MainForm MainForm { get; set; }
        public void LoadData()
        {
            
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT sa.saleid, sl.sellername, p.productname, sa.[date], sa.quantity\r\nFROM Sales sa\r\nINNER JOIN Products p on sa.productid = p.productid\r\nINNER JOIN Sellers sl ON sl.sellerid = sa.sellerid", con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }

        public void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 5) { 
                int id = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                new SaleUpdate { SaleEdit = this, SaleId = id }.ShowDialog();
            
            }
            if(e.ColumnIndex == 6) {
                int id = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
                {
                    using(SqlCommand cmd = new SqlCommand("DELETE sales WHERE saleid =@i", con))
                    {
                        cmd.Parameters.AddWithValue("@i", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        LoadData();
                    }
                }
            }
        }

        private void SalesEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.MainForm.LoadData();
        }
    }
}
