using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Shopperholics.ViewModels
{
    public class RegisterViewModel : LoginViewModel
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string firstname { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Please select a role")]
        public string RoleName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter email address")]
        public string loginemail { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a password.")]
        public string password { get; set; }

        [Display(Name = "Phone"), DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Please enter phone Number")]
        public string phoneno { get; set; }

        [Required(ErrorMessage = "Please enter address")]
        public string Address { get; set; }


    }
}
