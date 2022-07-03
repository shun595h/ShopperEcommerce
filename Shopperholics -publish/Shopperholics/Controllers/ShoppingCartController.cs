using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopperholics.Data;
using Shopperholics.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shopperholics.Repositories;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;





namespace Shopperholics.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ShopperholicsRepository _repository;
        private List<Products> products;
        private SessionStateViewModel sessionModel;
        private ShopperholicsContext _context;
        IHostingEnvironment _environment;

        /*public ShoppingCartController(ShopperholicsRepository repository)
        {
            _repository = repository;
        }*/
        public ShoppingCartController(ShopperholicsContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("productinfo")) && !string.IsNullOrEmpty(HttpContext.Session.GetString("CustomerProducts")))
            {
                List<int> productsListId = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("CustomerProducts"));
                products = new List<Products>();
                foreach (var item in productsListId)
                {
                    //var product = _repository.GetProducts().SingleOrDefault(p => p.id == item);
                    var product = _context.Products.SingleOrDefault(p => p.id == item);
                    products.Add(product);
                }
                sessionModel = new SessionStateViewModel
                {
                    productname = HttpContext.Session.GetString("productinfo"),
                    SelectedProducts = products
                };
                totalprice();
                return View(sessionModel);
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("productinfo")) && !string.IsNullOrEmpty(HttpContext.Session.GetString("CustomerProducts")))
            {
                List<int> productsListId = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("CustomerProducts"));
                products = new List<Products>();
                foreach (var item in productsListId)
                {
                    //var product = _repository.GetProducts().SingleOrDefault(p => p.id == item);
                    var product = _context.Products.SingleOrDefault(p => p.id == item);
                    products.Add(product);
                }
                foreach (var item in productsListId)
                {


                    products.RemoveAll(i => i.id == id);

                }


                var serialisedDate = JsonConvert.SerializeObject(products);
                HttpContext.Session.SetString("CustomerProducts", serialisedDate);
                if (serialisedDate.Length < 1)
                {
                    HttpContext.Session.Remove("productinfo");
                    HttpContext.Session.Remove("totalprice");
                }
                sessionModel = new SessionStateViewModel
                {
                    productname = HttpContext.Session.GetString("productinfo"),
                    SelectedProducts = products
                };



            }
            return View();


        }
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("productinfo");
            HttpContext.Session.Remove("totalprice");
            HttpContext.Session.Remove("CustomerProducts");
            HttpContext.Session.Remove("productdesc");
            HttpContext.Session.Remove("productprice");

            return RedirectToAction(nameof(Index));
        }
        public void totalprice()
        {
            double totalamt = 0;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("productinfo")) && !string.IsNullOrEmpty(HttpContext.Session.GetString("CustomerProducts")))
            {
                List<int> productsListId = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("CustomerProducts"));
                products = new List<Products>();
                foreach (var item in productsListId)
                {
                    //var product = _repository.GetProducts().SingleOrDefault(p => p.id == item);
                    var product = _context.Products.SingleOrDefault(p => p.id == item);
                    totalamt += product.Price;
                }
                HttpContext.Session.SetString("totalprice", totalamt.ToString());

            }

        }
        public IActionResult Checkout()
        {
            var credit = HttpContext.Session.GetString("usercredit");
            double creditamt = Convert.ToDouble(credit);
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("totalprice")))
            {
                var getuserid = HttpContext.Session.GetString("userid");
                var gettotalprice = HttpContext.Session.GetString("totalprice");
                double totalprice = Convert.ToDouble(gettotalprice);
                int userid = Convert.ToInt32(getuserid);
                if(creditamt > totalprice)
                {
                    creditamt -= totalprice;
                    var customertoupdate = _context.Customers.FirstOrDefault(c => c.username == User.Identity.Name);
                    using (var db = _context)
                    {
                        var result = db.Customers.SingleOrDefault(b => b.username == User.Identity.Name);
                        if (result != null)
                        {
                            List<int> productsListId = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("CustomerProducts"));
                            products = new List<Products>();
                            foreach (var item in productsListId)
                            {
                                var product = _context.Products.SingleOrDefault(p => p.id == item);
                                CustomerOrder co = new CustomerOrder
                                {
                                    Productid = product.id,
                                    CustomerId = userid,

                                };
                                _context.Add(co);
                                db.SaveChanges();

                            }

                                result.credit = creditamt;
                            
                            
                            db.SaveChanges();
                            HttpContext.Session.SetString("usercredit", creditamt.ToString());
                        }
                    }
                    
                    
                  
                }
                else
                {
                    
                }

            }
            return RedirectToAction(nameof(Index));

        }
        public IActionResult GetImage(int id)
        {
          
            Products requestedproduct =  _context.Products.SingleOrDefault(i => i.id == id);
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
    }
}
