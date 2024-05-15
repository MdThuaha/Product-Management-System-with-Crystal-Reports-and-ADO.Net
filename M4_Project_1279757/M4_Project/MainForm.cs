using M4_Project.ProductForms;
using M4_Project.Reports;
using M4_Project.SalesForms;
using M4_Project.SellerForms;
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
    public partial class MainForm : Form
    {
        DataSet ds;
        BindingSource bsP = new BindingSource();
        BindingSource bsS = new BindingSource();
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadData();
            
        }

        public void LoadData()
        {
            ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using(SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Products", con))
                {
                    da.Fill(ds, "products");
                    da.SelectCommand.CommandText = @"SELECT sa.[date], sl.sellername, sa.quantity, sa.productid
                                                    FROM Sales sa INNER JOIN Sellers sl ON sa.sellerid = sl.sellerid";
                    da.Fill(ds, "sales");
                    ds.Relations.Add(new DataRelation("FK_PROD_SALES", ds.Tables["products"].Columns["productid"], ds.Tables["sales"].Columns["productid"]));
                    ds.Tables["products"].Columns.Add(new DataColumn("image", typeof(byte[])));
                    for(int i = 0;i< ds.Tables["products"].Rows.Count;i++)
                    {
                        ds.Tables["products"].Rows[i]["image"]= File.ReadAllBytes(Path.Combine(@"..\..\Pictures", ds.Tables["products"].Rows[i]["picture"].ToString()));
                    }
                    bsP.DataSource = ds;
                    bsP.DataMember = "products";
                    bsS.DataSource = bsP;
                    bsS.DataMember = "FK_PROD_SALES";
                }
            }
            SetDataBindings();
        }

        private void SetDataBindings()
        {
           
            //
            this.lblName.DataBindings.Clear();
            this.lblName.DataBindings.Add(new Binding("Text", bsP, "productname"));
            this.lblPrice.DataBindings.Clear();
            Binding bp = new Binding("Text", bsP, "unitprice", true);
            bp.Format += Bp_Format;
            this.lblPrice.DataBindings.Add(bp);
            this.pictureBox1.DataBindings.Clear();
            this.pictureBox1.DataBindings.Add(new Binding("Image", bsP, "image", true));
            //
            this.dataGridView1.DataSource = bsS;
        }

        private void Bp_Format(object sender, ConvertEventArgs e)
        {
            decimal d = (decimal)e.Value;
            e.Value = d.ToString("0.00");
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CreatProduct {  MainForm=this}.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(bsP.Position < this.bsP.Count - 1)
            {
                bsP.MoveNext();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bsP.Position > 1)
            {
                bsP.MovePrevious();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bsP.MoveFirst();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bsP.MoveLast();
        }

        private void addToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            new CreateSales {  MainForm=this}.ShowDialog();
        }

        private void editDeleteToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            new SalesEdit { MainForm=this}.ShowDialog();
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new SellerCreate().ShowDialog();
        }

        private void editDeleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new SellerEdit().ShowDialog();
        }

        private void editDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ProductEdit().ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void productListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormProductReport().ShowDialog();
        }

        private void salesByProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormSalesByProduct().ShowDialog();
        }

        private void subReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormSubReport().ShowDialog();
        }

        private void masterDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MasterDetail().ShowDialog();
        }
    }
}
