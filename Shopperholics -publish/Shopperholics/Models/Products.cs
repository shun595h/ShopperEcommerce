using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace Shopperholics.Models
{
    public class Products
    {

       // public Products product { get; set; }
        [Key]
        public int id { get; set; }

        public int cid { get; set; }//customer id for customer selling the product or listing it
        
        public string ProductName { get; set; }

      //  [Range(0, 30)]
       // public int Quantity { get; set; }
        public productType? ProductType { get; set; }

        public string Description { get; set; }

        [Range(0.1, 100000)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [NotMapped]
        [Display(Name = "Picture")]
        public IFormFile ProductPhoto { get; set; }

        public string ImageName { get; set; }

        public byte[] PhotoFile { get; set; }


        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }

        [Required(ErrorMessage = "Please select a Category of your item")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Please select a Sub Category of your item")]
        public int? subCategoryId { get; set; }

        [Display(Name = "Last viewed on")]
        [NotMapped]
        public DateTime clickedonTimeDate { get; set; }

        [InverseProperty("Product")] //<< not sure 
        public virtual List<CustomerOrder> CustomerOrders { get; set; } //customer's orders adding to cart

        [NotMapped]
        public List<int> customersOrdersid { get; set; }


        [ForeignKey("CategoryId")]
        [InverseProperty("productss")]
        public  productCategory MenuCategory { get; set; }

        //[ForeignKey("subCategoryId")]
        //[InverseProperty("productscats")]
        public virtual productsubcategory subMenuCategory { get; set; }

       public virtual Customers customer { get; set; }

        public int? VendorId { get; set; }
        public virtual Vendors vendor { get; set; }


    }
}
