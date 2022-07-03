using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Shopperholics.Models
{
    public class Customers 
    {
        [Key]
        public int CustomerId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string firstname { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter your email address")]
        public string emailCustomer { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter your password.")]
        public string Password { get; set; }

        [Display(Name = "Phone"), DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Please enter phone Number")]
        public int phoneno { get; set; }

        [Required(ErrorMessage = "Please enter your adress")]
        public string Address { get; set; }

        public string username { get; set; }
        public double? credit { get; set; } //customer purchasing power

      
       
        public virtual List<CustomerOrder> customersOrders { get; set; }

        

        [NotMapped]
        [Display(Name = "Products List")]
        public List<int> SelectedProductsList { get; set; }

        public virtual ICollection<Products> products { get; set; }
        

    }
}
