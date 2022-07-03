using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Shopperholics.Data;
using Shopperholics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.AspNetCore.Http;
namespace Shopperholics.Repositories
{
    public class ShopperholicsRepository : IShopperholicsRepository
    {
        private ShopperholicsContext _scontext;
        private IConfiguration _configuration;
        private CloudBlobContainer _container;
        public ShopperholicsRepository(ShopperholicsContext scontext, IConfiguration configuration)
        {
            _scontext = scontext;
            _configuration = configuration;
           // string connectionString = _configuration.GetConnectionString("AzureStorageConnectionString-1");
         //   string containerName = _configuration.GetValue<string>("ContainerSettings:ContainerName");
          //  CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
           // CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
           // _container = blobClient.GetContainerReference(containerName);
        }
        public IEnumerable<CustomerOrder> GetOrders()
        {
            return _scontext.CustomerOrders.ToList();
        }

        public IEnumerable<Products> GetProducts()
        {
            return _scontext.Products.ToList();
        }

        public IEnumerable<productCategory> getProductCategory()
        {
            return _scontext.menucategories.ToList();
        }

        public IEnumerable<productsubcategory> GetProductsubcategories()
        {
            return _scontext.submenucategories.ToList();
        }

        public CustomerOrder GetOrderbyId(int id)//customer order
        {
            return _scontext.CustomerOrders.Include(o => o.Product)
                .SingleOrDefault(c => c.Productid == id);
        }

         public Products GetProductsbyId(int id)//retrieving the product list //are we getting the product list to display right?
         {
          
            return _scontext.Products.Include(v => v.vendor)
           .SingleOrDefault(c => c.id == id);

            
          
        }

        public Products GetProductsCatbyId(int id)
        {
            return _scontext.Products.Include(b => b.MenuCategory)
                .SingleOrDefault(c => c.CategoryId == id);
        }

        public productCategory GetProductSubCategory(int id)
        {
            return _scontext.menucategories.Include(m => m.subMenuCategory)
                .SingleOrDefault(s => s.subCategoryId == id);
        }

        public void AddCustomers(Customers customer)
        {
            _scontext.Add(customer);
            _scontext.SaveChanges();
        }
        public void AddOrder(CustomerOrder order)
        {
            _scontext.Add(order);
            _scontext.SaveChanges();
        }
        public void AddProduct(Products product)
        {
            if (product.ProductPhoto != null && product.ProductPhoto.Length > 0)
            {
                product.ImageMimeType = product.ProductPhoto.ContentType;
                product.ImageName = Path.GetFileName(product.ProductPhoto.FileName);
               /* string imageURL = UploadImageAsync(product.ProductPhoto).GetAwaiter().GetResult();
                product.ImageName = imageURL;
                product.ImageMimeType = product.ProductPhoto.ContentType;
                product.ImageName = Path.GetFileName(product.ProductPhoto.FileName);*/
                using (var memoryStream = new MemoryStream())
                {
                    product.ProductPhoto.CopyTo(memoryStream);
                    product.PhotoFile = memoryStream.ToArray();
                }
                _scontext.Add(product);
                _scontext.SaveChanges();
            }
        }
        public void AddProductCategory(productCategory productcat) //addproductcategory is for the admin only
        {
            if (productcat.categoryPhoto != null && productcat.PhotoFile.Length > 0)
            {
                productcat.ImageMimeType = productcat.categoryPhoto.ContentType;
                productcat.ImageName = Path.GetFileName(productcat.categoryPhoto.FileName);
                using (var memoryStream = new MemoryStream())
                {
                    productcat.categoryPhoto.CopyTo(memoryStream);
                    productcat.PhotoFile = memoryStream.ToArray();
                }
                _scontext.Add(productcat);
                _scontext.SaveChanges();
            }


        }
        public void AddProductSubCategory(productsubcategory productsubcat)
        {
            if (productsubcat.subcatPhoto != null && productsubcat.PhotoFile.Length > 0)
            {
                productsubcat.ImageMimeType = productsubcat.subcatPhoto.ContentType;
                productsubcat.ImageName = Path.GetFileName(productsubcat.subcatPhoto.FileName);
                using (var ms = new MemoryStream())
                {
                    productsubcat.subcatPhoto.CopyTo(ms);
                    productsubcat.PhotoFile = ms.ToArray();
                }
                _scontext.Add(productsubcat);
                _scontext.SaveChanges();
            }

        }
        public void RemoveOrder(int id)
        {
            var order = _scontext.CustomerOrders.SingleOrDefault(o => o.Productid == id);
            _scontext.CustomerOrders.Remove(order);
            _scontext.SaveChanges();
        }
        public void removeProduct(int id)
        {
            var product = _scontext.Products.SingleOrDefault(p => p.id == id);
            _scontext.Products.Remove(product);
           
          /*  if (product.ImageName != null)
            {
                DeleteImageAsync(product.ImageName).GetAwaiter().GetResult();
            }*/
            _scontext.SaveChanges();
        }
        public void removeProductCategory(int id)
        {
            var productcat = _scontext.menucategories.SingleOrDefault(m => m.id == id);
            _scontext.menucategories.Remove(productcat);
            _scontext.SaveChanges();

        }
        public void RemoveProductsubCategory(int id)
        {
            var productsubcat = _scontext.submenucategories.SingleOrDefault(sm => sm.id == id);
            _scontext.submenucategories.Remove(productsubcat);
            _scontext.SaveChanges();
        }

        public void SaveChanges()
        {
            _scontext.SaveChanges();
        }

        public IQueryable<CustomerOrder> PopulateCustomerOrdersdropdownList()
        {
            var customerOrderQuery = from c in _scontext.CustomerOrders
                                     orderby c.Product
                                     select c;

            return customerOrderQuery;
        }
       public IQueryable<Products> PopulateProductsDropDownList()
        {
            var productsQuery = from c in _scontext.Products
                                orderby c.ProductName
                                select c;
            return productsQuery;
        }
       public IQueryable<productCategory> PopulateProductCategories()
        {
            var productcatQuery = from c in _scontext.menucategories
                                  orderby c.Name
                                  select c;

            return productcatQuery;
        }

        public IQueryable<productsubcategory> PopulatesubProductCategories()
        {
            var productsubcat = from c in _scontext.submenucategories                             
                                orderby c.Name
                                select c;
            return productsubcat;
        }
        public IQueryable<Vendors> PopulateProductVendors()
        {

            var productvendor = from c in _scontext.Vendors
                                orderby c.VendorName
                                select c;
            return productvendor;
        }
        private async Task<string> UploadImageAsync(IFormFile photo)
        {
            CloudBlockBlob blob = _container.GetBlockBlobReference(Path.GetFileName(photo.FileName));
            await blob.UploadFromStreamAsync(photo.OpenReadStream());
            return blob.Uri.ToString();
        }
        private async Task<bool> DeleteImageAsync(string PhotoFileName)
        {
            CloudBlockBlob blob = _container.GetBlockBlobReference(PhotoFileName);
            await blob.DeleteAsync();
            return true;
        }
    }
}
