using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Shopperholics.Models
{
    public class User : IdentityUser
    {
        
   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public int cid { get; set; }
        public double Credits { get; set; }
        public virtual ICollection<CustomerOrder> orders { get; set; }
    }
   
}
