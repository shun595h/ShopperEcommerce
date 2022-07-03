using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Shopperholics.Models
{
    public class productCategory
    {
        [Key]
        public int id { get; set; }

        public int subCategoryId { get; set; }
        public string Name { get; set; }

        [NotMapped]
        [Display(Name = "Picture")]
        public IFormFile categoryPhoto { get; set; }

        public string ImageName { get; set; }

        public byte[] PhotoFile { get; set; }

        
        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }

        public List<Products> productss { get; set; }


        public productsubcategory subMenuCategory { get; set; }

        public virtual ICollection<Products> Productss { get; set; }

        public virtual ICollection<productsubcategory> subcat { get; set; }


    }
}
