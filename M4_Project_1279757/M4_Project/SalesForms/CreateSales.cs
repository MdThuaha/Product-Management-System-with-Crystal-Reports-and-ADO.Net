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
    public partial class CreateSales : Form
    {
        public CreateSales()
        {
            InitializeComponent();
        }

        private void CreateSales_Load(object sender, EventArgs e)
        {
            LoadCombo();

        }
        public MainForm MainForm {  get; set; }
        private void LoadCombo()
        {
            using(SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM products", con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.SelectCommand.CommandText = "SELECT * from sellers";
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);
                    comboBox1.DataSource = dt;
                    comboBox2.DataSource = dt2;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using(SqlCommand cmd = new SqlCommand("INSERT INTO sales ([date], sellerid, productid, quantity) VALUES (@d, @s, @p, @q)", con))
                {
                    cmd.Parameters.AddWithValue("@d", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@s", comboBox2.SelectedValue);
                    cmd.Parameters.AddWithValue("@p", comboBox1.SelectedValue);
                    cmd.Parameters.AddWithValue("@q", numericUpDown1.Value);
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Data saved", "Success");
                        dateTimePicker1.Value = DateTime.Now;
                        numericUpDown1.Value = 0;

                    }
                }
               
            }
        }

        private void CreateSales_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.LoadData();
        }
    }
}
