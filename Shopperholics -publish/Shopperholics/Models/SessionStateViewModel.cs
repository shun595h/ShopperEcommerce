using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopperholics.Models
{
    public class SessionStateViewModel
    {
        public string productname { get; set; }

        public string productdesc { get; set; }
        public double productprice { get; set; }

        public List<Products> SelectedProducts { get; set; }
    }
}
