using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shopperholics.Models
{
    public class productsubcategory
    {
        [Key]
        public int id { get; set; }

      public int productcatid { get; set; }
        public string Name { get; set; }

        [Display(Name = "Picture")]
        [NotMapped]
        public IFormFile subcatPhoto { get; set; }

        public string ImageName { get; set; }

        public byte[] PhotoFile { get; set; }


        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }
        public List<productCategory> productscat { get; set; }
        
      

        public virtual productCategory Productscat { get; set; }

    }
}
