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


namespace AnimalsMvc.Controllers
{
  
    public class HomeController : Controller
    {
        private IShopperholicsRepository _repository;
        private IHostingEnvironment _environment;
        private ShopperholicsContext _context;

        public HomeController(IShopperholicsRepository repository, IHostingEnvironment environment, ShopperholicsContext context)
        {
            _repository = repository;
            _environment = environment;
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_repository.GetProducts());
        }

       
        public IActionResult Accounts()
        {

            return View();
        }

       

       
    }
}
