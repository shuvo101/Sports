using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hanssens.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SportsWeb.Models;
using SportsWeb.Repository;
using SportsWeb.ViewModels;

namespace SportsWeb.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var token = string.Empty;
            var loggedInUserInfo = new LoggedInUserInfo();
            using (var storage = new LocalStorage())
            {
                if (storage.Exists("userInfo"))
                {
                    loggedInUserInfo = JsonConvert.DeserializeObject<LoggedInUserInfo>(storage.Get("userInfo").ToString());
                    token = storage.Get("jwtToken").ToString();
                }
            }

            if (string.IsNullOrEmpty(token))
            {
                using (var storage = new LocalStorage())
                {
                    storage.Clear();
                }
                return View();
            }

            PayloadResponse response = new PayloadResponse();
            using (var repository = new WebApiClientRepository<PayloadResponse>())
            {
                try
                {
                    response = repository.GlobalApiCallPost(null, "api/Auth/KeepAlive");

                    if (response == null || !response.Success)
                    {
                        using (var storage = new LocalStorage())
                        {
                            storage.Clear();
                        }
                        return View();
                    }
                }
                catch (Exception)
                {
                    return View();
                }
            }

            if (loggedInUserInfo != null && loggedInUserInfo.UserID > 0)
            {
                switch (loggedInUserInfo.Role)
                {
                    case "Coach": return RedirectToAction(nameof(CoachController.Index), "Coach");

                    case "Athlete":
                        return RedirectToAction(nameof(AthleteController.Index), "Athlete");

                    default:
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            LoginResponseViewModel response = new LoginResponseViewModel();
            using (var repository = new WebApiClientRepository<LoginResponseViewModel>())
            {
                var payload = repository.GlobalApiCallPost(model, "login");
                if (payload != null)
                {
                    if (payload.successResonse != null)
                    {
                        string jwtToken = payload.successResonse.Token;
                        var userInfo = payload.successResonse.User;
                        using (var storage = new LocalStorage())
                        {
                            storage.Store("jwtToken", jwtToken);
                            storage.Store("userInfo", userInfo);
                        }
                        switch (userInfo.Role)
                        {
                            case "Coach": return RedirectToAction(nameof(CoachController.Index), "Coach");

                            case "Athlete":
                                return RedirectToAction(nameof(AthleteController.Index), "Athlete");

                            default:
                                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                                return View();
                        }

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult LogOff(LoginViewModel model, string returnUrl = null)
        {
            using (var storage = new LocalStorage())
            {
                storage.Clear();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}