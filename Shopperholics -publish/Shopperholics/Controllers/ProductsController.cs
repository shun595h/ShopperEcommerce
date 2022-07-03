using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shopperholics.Models;
using Shopperholics.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Shopperholics.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace Shopperholics.Controllers
{
   
    public class ProductsController : Controller
    {
        private IShopperholicsRepository _repository;
        private IHostingEnvironment _environment;
        private ShopperholicsContext _context;
        

        public ProductsController(IShopperholicsRepository repository, IHostingEnvironment environment, ShopperholicsContext context)
        {
            _repository = repository;
            _environment = environment;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            
            return View(_repository.GetProducts());
        }
       

        public IActionResult Details(int id)
        {

            var product = _repository.GetProductsbyId(id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Create(int id)
        {
            PopulatVendorDropDownList();
            PopulateProductsCatDropDownList();
            PopulateProductsSubCatDropDownList();
          //  if(id > 0)
          //    {
          //  PopulateSubCatDropDownlist(1);
          //   }


            return View();
        }
       

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Create")]
        public IActionResult CreateProduct(Products product)
        {
            if(ModelState.IsValid)
            {
                _repository.AddProduct(product);
                return RedirectToAction(nameof(Index));
            }
            PopulatVendorDropDownList(product.VendorId);
            PopulateProductsCatDropDownList(product.CategoryId);
            PopulateProductsSubCatDropDownList(product.CategoryId);
            return View(product);
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Products product = _repository.GetProductsbyId(id);

            if(product == null)
            {
                return NotFound();
            }
            PopulateProductsCatDropDownList(product.CategoryId);
            PopulateProductsSubCatDropDownList(product.subCategoryId);
            return View(product);
        }
        
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Edit")]
        public async Task<ActionResult> EditProduct(int id)
        {
            var productToUpdate = _repository.GetProductsbyId(id);
            if (productToUpdate.ProductPhoto != null && productToUpdate.PhotoFile.Length > 0)
            {
                productToUpdate.ImageMimeType = productToUpdate.ProductPhoto.ContentType;
                productToUpdate.ImageName = Path.GetFileName(productToUpdate.ProductPhoto.FileName);
                using (var memoryStream = new MemoryStream())
                {
                    productToUpdate.ProductPhoto.CopyTo(memoryStream);
                    productToUpdate.PhotoFile = memoryStream.ToArray();
                }
               
            }
            bool isUpdated = await TryUpdateModelAsync<Products>(
                productToUpdate,
                    "",
                    p => p.CategoryId,
                    p => p.subCategoryId,
                    p => p.ProductName,
                    p => p.ProductType,
                    p => p.Description,
                    p => p.Price,
                    p => p.PhotoFile,
                    p => p.ProductPhoto,
                    p => p.ImageMimeType,
                    p => p.ImageName
                    );

            if(isUpdated)
            {
                _repository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            PopulatVendorDropDownList(productToUpdate.VendorId);         
            PopulateProductsCatDropDownList(productToUpdate.CategoryId);
            PopulateProductsSubCatDropDownList(productToUpdate.subCategoryId);
            return View(productToUpdate);
                    
            
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _repository.GetProductsbyId(id);
            if(product ==null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        public IActionResult RemoveProduct(int id)
        {
            _repository.removeProduct(id);
            return RedirectToAction(nameof(Index));
        }


        private void PopulatVendorDropDownList(int? selectedProductVendor = null)
        {
            var vendors = _repository.PopulateProductVendors();

            ViewBag.VendorId = new SelectList(vendors.AsNoTracking(), "VendorId", "VendorName", selectedProductVendor);
        }
        private void PopulateProductsCatDropDownList(int? selectedProductCat = null)
        {
            var productcat = _repository.PopulateProductCategories();
            ViewBag.ProductcatID = new SelectList(productcat.AsNoTracking(),"id","Name", selectedProductCat);
        }

        private void PopulateProductsSubCatDropDownList(int? selectedProductSubCat = null)
        {
            var productcat = _repository.PopulatesubProductCategories();
            ViewBag.ProductsubcatID = new SelectList(productcat.AsNoTracking(), "id", "Name", selectedProductSubCat);
        }
        public void PopulateSubCatDropDownlist(int Id)
        {
            
            var category = _context.submenucategories.FirstOrDefault(c => c.productcatid == Id);
            ViewBag.getcatid = category.Name;
          



        }
      

        public IActionResult GetImage(int id)
        {
            Products requestedproduct = _repository.GetProductsbyId(id);
            if (requestedproduct != null)
            {
                string webRootPath = _environment.WebRootPath;
                string folderPath = "\\images\\";
                string fullpath = webRootPath + folderPath + requestedproduct.ImageName;
                if (System.IO.File.Exists(fullpath))
                {
                    FileStream fileOnDisk = new FileStream(fullpath, FileMode.Open);
                    byte[] fileBytes;
                    using (BinaryReader br = new BinaryReader(fileOnDisk))
                    {
                        fileBytes = br.ReadBytes((int)fileOnDisk.Length);
                    }
                    return File(fileBytes, requestedproduct.ImageMimeType);
                }
                else
                {
                    if (requestedproduct.PhotoFile.Length > 0)
                    {
                        return File(requestedproduct.PhotoFile, requestedproduct.ImageMimeType);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult GetByCategory(int id)
        {
            var products = _repository.GetProducts().Where(p => p.CategoryId == id);
            var category = _repository.getProductCategory().First(p => p.id == id);
            ViewBag.categoryTitle = category;
            return View(products);
        }

        [HttpGet]
        public IActionResult AddToShoppingcart()
        {
            if(HttpContext.Session.GetString("productinfo") != null)
            {
                Products sessionproducts = new Products()
                {

                    ProductName = HttpContext.Session.GetString("productinfo"),
                    Description = HttpContext.Session.GetString("productdesc"),
                    Price = Convert.ToInt32(HttpContext.Session.GetString("productprice")),
                  
                 
                };
                //populate
                return View(sessionproducts);
            }
            //populate
            return View();
        }

        [HttpPost, ActionName("AddToShoppingList")]
        public IActionResult AddToShoppingListPost(Products products)
        {
            products.customersOrdersid = new List<int>();
          
              
                HttpContext.Session.SetInt32("productinfo", products.id);
                HttpContext.Session.SetString("productdesc", products.Description);
                HttpContext.Session.SetString("productprice", Convert.ToString(products.Price));


                if (HttpContext.Session.GetString("CustomerProducts") != null)
                {



                    List<int> productsListId = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("CustomerProducts"));
                    if (!productsListId.Contains(products.id))
                    {
                        products.customersOrdersid.AddRange(productsListId);
                        products.customersOrdersid.Add(products.id);
                        var serialisedDate = JsonConvert.SerializeObject(products.customersOrdersid);
                        HttpContext.Session.SetString("CustomerProducts", serialisedDate);
                        return RedirectToAction(nameof(Index));
                    }
                  

                }
                else
                {

                    products.customersOrdersid.Add(products.id);
                    var serialisedDate = JsonConvert.SerializeObject(products.customersOrdersid);
                    HttpContext.Session.SetString("CustomerProducts", serialisedDate);
                    
                }
                return RedirectToAction(nameof(Index));
           
        }




    }
}
