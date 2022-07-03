using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopperholics.Models;
using Shopperholics.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Shopperholics.Data;
using Microsoft.AspNetCore.Http;

namespace Shopperholics.Controllers
{
    public class CustomerController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ShopperholicsContext _context;


        public CustomerController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ShopperholicsContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Products");
            }
            return View();
        }

        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> LoginPost(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, loginModel.RememberMe, false);
                if (result.Succeeded)
                {
                    var customer = _context.Customers.FirstOrDefault(c => c.username == loginModel.UserName);
                    HttpContext.Session.SetString("usercredit", customer.credit.ToString());
                    HttpContext.Session.SetString("userid", customer.CustomerId.ToString());
                    return RedirectToAction("Index", "Products");
                }
            }
            ModelState.AddModelError("", "Faild to Login");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Products");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ActionName("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPost(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                Customers customer = new Customers
                {
                    firstname = registerModel.firstname,
                    LastName = registerModel.LastName,
                    emailCustomer = registerModel.loginemail,
                    Password = registerModel.password,
                    phoneno = Convert.ToInt32(registerModel.phoneno),
                    Address = registerModel.Address,
                    username = registerModel.UserName,


                };
                User user = new User
                {
                    FirstName = registerModel.firstname,
                    LastName = registerModel.LastName,
                    UserName = registerModel.UserName,
                    PhoneNumber = registerModel.phoneno,
                    Email = registerModel.loginemail,

                };

                var result = await _userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {

                    bool roleExists = await _roleManager.RoleExistsAsync(registerModel.RoleName);
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(registerModel.RoleName));
                    }

                    if (!await _userManager.IsInRoleAsync(user, registerModel.RoleName))
                    {
                        await _userManager.AddToRoleAsync(user, registerModel.RoleName);
                    }

                    if (!string.IsNullOrWhiteSpace(user.Email))
                    {
                        Claim claim = new Claim(ClaimTypes.Email, user.Email);
                        await _userManager.AddClaimAsync(user, claim);
                    }

                    var resultSignIn = await _signInManager.PasswordSignInAsync(registerModel.UserName, registerModel.Password, registerModel.RememberMe, false);
                    if (resultSignIn.Succeeded)
                    {

                        _context.Add(customer);
                        _context.SaveChanges();
                        return RedirectToAction("Index", "Products");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        public IActionResult AccessDenied()
        {

            return View();
        }

        public IActionResult MyAccount()
        {
            return View();
        }
        public async Task<IActionResult> orders()
        {

            var res = from co in _context.CustomerOrders
                      join p in _context.Products
                      on co.Productid equals p.id
                      select p;

            return View(res);

            

        }
    }
}