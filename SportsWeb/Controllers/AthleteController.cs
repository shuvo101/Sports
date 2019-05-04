using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hanssens.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SportsWeb.Repository;
using SportsWeb.ViewModels;

namespace SportsWeb.Controllers
{
    public class AthleteController : Controller
    {
        public IActionResult Index()
        {
            AthleteTestViewModel model = new AthleteTestViewModel();
            using (var repository = new WebApiClientRepository<AthleteTestViewModel>())
            {
                int userID = 0;
                using (var storage = new LocalStorage())
                {
                    LoggedInUserInfo userInfo = JsonConvert.DeserializeObject<LoggedInUserInfo>(storage.Get("userInfo").ToString());
                    userID = userInfo.UserID;
                }
                var payload = repository.GlobalApiCallGet(null, "api/Test/GetTestListByAthlete?id=" + userID);
                if (payload != null)
                {
                    model = payload;
                    return View(model);
                }
            }
            return View(model);
        }
    }
}