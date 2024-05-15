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
    public partial class SaleUpdate : Form
    {
        public SaleUpdate()
        {
            InitializeComponent();
        }
        public int SaleId {  get; set; }
        public SalesEdit SaleEdit { get; set; }
        private void LoadCombo()
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM products", con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.SelectCommand.CommandText = "SELECT * from sellers";
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);
                    comboBox1.DataSource = dt2;
                    comboBox2.DataSource = dt;
                }
            }
        }

        private void SaleUpdate_Load(object sender, EventArgs e)
        {
            LoadCombo();
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using(SqlCommand cmd = new SqlCommand("SELECT * FROM sales WHERE saleid=@i", con))
                {
                    cmd.Parameters.AddWithValue("@i", SaleId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if(reader.Read())
                    {
                        lblId.Text = reader["saleid"].ToString();
                        comboBox1.SelectedValue= reader["sellerid"].ToString();
                        comboBox2.SelectedValue = reader["productid"].ToString();
                        dateTimePicker1.Value = DateTime.Parse(reader["date"].ToString());
                        numericUpDown1.Value = decimal.Parse(reader["quantity"].ToString());
                    }
                    con.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE sales SET [date]=@d, productid=@p, sellerid=@s, quantity=@q WHERE saleid=@i", con))
                {
                    cmd.Parameters.AddWithValue("@d", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@p", comboBox2.SelectedValue);
                    cmd.Parameters.AddWithValue("@s", comboBox1.SelectedValue);
                    cmd.Parameters.AddWithValue("@q", numericUpDown1.Value);
                    cmd.Parameters.AddWithValue("@i", SaleId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Data saved", "Success");
                    this.SaleEdit.LoadData();
                }

            }
        }
    }
}
