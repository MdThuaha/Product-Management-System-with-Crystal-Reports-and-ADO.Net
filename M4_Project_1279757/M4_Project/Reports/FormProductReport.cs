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

namespace M4_Project.Reports
{
    public partial class FormProductReport : Form
    {
        public FormProductReport()
        {
            InitializeComponent();
        }

        private void FormProductReport_Load(object sender, EventArgs e)
        {
            RptProduct rpt = new RptProduct();
            DataSet ds = new DataSet(); 
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Products", con))
                {
                    da.Fill(ds, "products1");
                  
                    ds.Tables["products1"].Columns.Add(new DataColumn("image", typeof(byte[])));
                    for (int i = 0; i < ds.Tables["Products1"].Rows.Count; i++)
                    {
                        ds.Tables["Products1"].Rows[i]["image"] = File.ReadAllBytes(Path.Combine(@"..\..\Pictures", ds.Tables["Products1"].Rows[i]["picture"].ToString()));
                    }
                    rpt.SetDataSource(ds);
                    crystalReportViewer1.ReportSource = rpt;
                    crystalReportViewer1.Refresh();
                }
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
