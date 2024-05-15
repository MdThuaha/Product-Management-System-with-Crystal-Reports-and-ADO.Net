using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M4_Project.Models
{
    public class Sale
    {
        public int saleid { get; set; }
        public DateTime date { get; set; }
        public int productid { get; set; }
        public int sellerid { get; set; }   
        public int quantity { get; set; }
    }
}
