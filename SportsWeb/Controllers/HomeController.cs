using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hanssens.Net;
using Microsoft.AspNetCore.Mvc;
using SportsWeb.Models;

namespace SportsWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string token = string.Empty;
            using (var storage = new LocalStorage())
            {
                token = storage.Get("jwtToken").ToString();
            }
            
            if (!string.IsNullOrEmpty(token))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Index");
            }
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
