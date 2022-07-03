using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Shopperholics.Models
{
    public class Vendors
    {
        [Key]
        public int VendorId { get; set; }

        [StringLength(50, MinimumLength = 4)]
        public string VendorName { get; set; }


        [StringLength(50, MinimumLength = 4)]
        public string Address { get; set; }

        public string vendorDescription { get; set; }

        public virtual ICollection<Products> vendorproducts { get; set; }
    }
}
