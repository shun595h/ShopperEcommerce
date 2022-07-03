using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopperholics.Models
{
    public class CustomerOrder
    {
        
        public int Productid { get; set; }
        public Products Product { get; set; }

        public int CustomerId { get; set; }



       
        public Customers customer { get; set;}

        
    }
}
