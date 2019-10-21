using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IMTestProject.Portal.Controllers
{
    public class AdditionalInformationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}