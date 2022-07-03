using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopperholics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Shopperholics.Data
{
    public class ShopperholicsContext : IdentityDbContext<User>
    {
        public ShopperholicsContext(DbContextOptions<ShopperholicsContext> options) : base (options)
        {
            

        }

      public DbSet<User> CustomerUsers { get; set; }
        public DbSet<Vendors> Vendors { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<productCategory> menucategories { get; set; }

        public DbSet<productsubcategory> submenucategories { get; set; }
        protected override void OnModelCreating(ModelBuilder shopmodelBuilder)
        {
            base.OnModelCreating(shopmodelBuilder);

            shopmodelBuilder.Entity<CustomerOrder>()
                .HasKey(c => new { c.Productid, c.CustomerId, });

            shopmodelBuilder.Entity<CustomerOrder>()
                .HasOne(c => c.customer) //one customer have many customer orders
                .WithMany(m => m.customersOrders)
                .HasForeignKey(fk => fk.CustomerId);

            shopmodelBuilder.Entity<CustomerOrder>()
                .HasOne(c => c.Product) //one product
                .WithMany(m => m.CustomerOrders)
                .HasForeignKey(fk => fk.Productid);

            shopmodelBuilder.Entity<Products>() //product has product cat
                .HasOne(c => c.MenuCategory)
                .WithMany(m => m.productss);

            shopmodelBuilder.Entity<productCategory>() //and product cat has sub product cat
               .HasOne(c => c.subMenuCategory)
               .WithMany(m => m.productscat);


            shopmodelBuilder.Entity<Vendors>().HasData(
              new Vendors
              {
                  VendorId = 1,
                  VendorName = "Good Works",
                  Address = "635 Brighton Circle Road",
                
              },
               new Vendors
               {
                   VendorId = 2,
                   VendorName = "Best Bargains",
                   Address = "1608 Charles Street",
                 
               },
               new Vendors
               {
                   VendorId = 3,
                   VendorName = "Quality",
                   Address = "2553 Pin Oak Drive",
                  
               }
            
           );
            shopmodelBuilder.Entity<Products>().HasData(
                 new Products
                 {
                     id = 1,
                     ProductName = "TV",
                     Description = "Smart TV 49 Inch",
                     Price = 850,
                     ImageMimeType = "image/jpeg",
                     ImageName = "waters.jpg",
                     CategoryId = 1,
                     subCategoryId = 1,
                     VendorId = 1
                 },
                new Products
                {
                    id = 2,
                    ProductName = "Coffee Machine",
                    Description = "Coffee maker",
                    Price = 200,
                    ImageMimeType = "image/jpeg",
                    ImageName = "swan.jpg",
                    CategoryId = 2,
                    subCategoryId = 1,
                    VendorId = 1

                },

                new Products
                {
                    id = 3,
                    ProductName = "Fax Machine",
                    Description = "Multifunction a printer and a fax machine",
                    Price = 390,
                    ImageMimeType = "image/jpeg",
                    ImageName = "secondads.jpg",
                    CategoryId = 2,
                    subCategoryId = 2,
                    VendorId = 2
                    
                },
                new Products
                {
                    id = 4,
                    ProductName = "Washer",
                    Description = "Automatic washing laundry machine",
                    Price = 1499,
                    ImageMimeType = "image/jpeg",
                    ImageName = "firstads.jpg",
                    CategoryId = 1,
                    subCategoryId = 2,
                    VendorId = 3
                    
                }); 


            shopmodelBuilder.Entity<productCategory>().HasData(
                  new productCategory
                  {
                      id = 1,
                      Name = "Furniture"
                    
                  },
                new productCategory
                {
                    id = 2,
                    Name = "Electrical Appliances"
                },
                new productCategory
                {
                    id = 3,
                    Name = "Fashion"
  
              
                });

            shopmodelBuilder.Entity<productsubcategory>().HasData(
                 new productsubcategory
                 {
                     id = 1,
                     productcatid = 1,
                     Name = "Chairs"
                 },
               new productsubcategory
               {
                   id = 2,
                   productcatid = 1,
                   Name = "Tables"
               },
               new productsubcategory
               {
                   id = 3,
                   productcatid = 2,
                   Name = "Kitchen"
                   
               },
                new productsubcategory
                {
                    id = 4,
                    productcatid = 2,
                    Name = "Living Room"

                },
                 new productsubcategory
                 {
                     id = 5,
                     productcatid = 3,
                     Name = "Top"
                 },
                 new productsubcategory
                 {
                     id = 6,
                     productcatid = 3,
                     Name = "Bottom"

                 },
                 new productsubcategory
                 {
                     id = 7,
                     productcatid = 3,
                     Name = "Shoes"


               });







        }
    }
   
}
