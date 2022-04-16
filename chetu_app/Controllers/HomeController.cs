using chetu_app.DB_Context;
using chetu_app.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace chetu_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Index(usermodel mod)
        {
            chetu_India_CompanyContext ent = new chetu_India_CompanyContext();
            var user = ent.LoginInformationTables.Where(m => m.Email == mod.Email).FirstOrDefault();
            if (user == null)
            {
                TempData["invalid"] = "Email.is not found invalid user ";
            }
            else
            {
                if (user.Email == mod.Email && user.Password == mod.Password)
                {
                    var claims = new[] {new Claim(ClaimTypes.Name,user.Name),
                    new Claim(ClaimTypes.Name,user.Name) };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authproperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity),
                        authproperties);
                   
                    HttpContext.Session.SetString("name",user.Name);
                    HttpContext.Session.GetString("name");
                    return RedirectToAction("Indexdashboard");
                   
                }
                else
                {
                    TempData["not valid"] = "wrong password";
                }
            }
            return View();
        }
        public IActionResult emp_list()
        {
            chetu_India_CompanyContext ent = new chetu_India_CompanyContext();
            List<empmodel> mod = new List<empmodel>();
            var res = ent.ChetuEmplyoyees.ToList();
            foreach (var item in res)
            {
                mod.Add(new empmodel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    City = item.City,
                    Designation = item.Designation,
                    Mobile = item.Mobile,
                });

            }
            return View(mod);
        }
        [HttpGet]
        public IActionResult add_employee()
        {
            return View();
        }
        [HttpPost]
        public IActionResult add_employee(empmodel mod)
        {
            chetu_India_CompanyContext ent = new chetu_India_CompanyContext();
            ChetuEmplyoyee tab = new ChetuEmplyoyee();
            tab.Id = mod.Id;
            tab.Name = mod.Name;
            tab.Email = mod.Email;
            tab.City = mod.City;
            tab.Designation = mod.Designation;
            tab.Mobile = mod.Mobile;
            if (mod.Id == 0)
            {
                ent.ChetuEmplyoyees.Add(tab);
                ent.SaveChanges();
            }
            else
            {
                ent.Entry(tab).State = EntityState.Modified;
                ent.SaveChanges();
            }
            return RedirectToAction("emp_list" ,"Home");
        }
        public IActionResult Edit(int id)
        {
            empmodel mod = new empmodel();
            chetu_India_CompanyContext ent = new chetu_India_CompanyContext();
            var edit = ent.ChetuEmplyoyees.Where(m => m.Id == id).First();
            mod.Id = edit.Id;
            mod.Name = edit.Name;
            mod.Email = edit.Email;
            mod.City = edit.City;
            mod.Designation = edit.Designation;
            mod.Mobile = edit.Mobile;

            return View("add_employee", mod);
        }
        public IActionResult Delete(int id)
        {
            chetu_India_CompanyContext ent = new chetu_India_CompanyContext();
            var dlt = ent.ChetuEmplyoyees.Where(m => m.Id == id).First();
            ent.ChetuEmplyoyees.Remove(dlt);
            ent.SaveChanges();
            return RedirectToAction("emp_list");
        }
        [HttpGet]
        public IActionResult user_registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult user_registration(usermodel mod)
        {
            chetu_India_CompanyContext ent = new chetu_India_CompanyContext();
            LoginInformationTable logtb = new LoginInformationTable();
            logtb.Id = mod.Id;
            logtb.Name = mod.Name;
            logtb.Email = mod.Email;
            logtb.Password = mod.Password;
            ent.LoginInformationTables.Add(logtb);
            ent.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Indexdashboard()
        {
            return View();
        }
        public IActionResult _leftnavbar()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
