using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hanssens.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SportsWeb.Models;
using SportsWeb.Repository;
using SportsWeb.ViewModels;

namespace SportsWeb.Controllers
{
    public class CoachController : Controller
    {
        public IActionResult Index()
        {
            TestViewModel model = new TestViewModel();
            List<TestListViewModel> testList = new List<TestListViewModel>();
            using (var repository = new WebApiClientRepository<List<TestListViewModel>>())
            {
                int userID = 0;
                using (var storage = new LocalStorage())
                {
                    LoggedInUserInfo userInfo = JsonConvert.DeserializeObject<LoggedInUserInfo>(storage.Get("userInfo").ToString());
                    userID = userInfo.UserID;
                }
                var payload = repository.GlobalApiCallGet(null, "api/Test/GetTestListByCoach?id=" + userID);
                if (payload != null)
                {
                    testList = payload.ToList();
                }
            }
            model.TestList = testList;
            var testTypeList = GetTestType();
            ViewBag.TestTypeID = new SelectList(testTypeList, "ID", "Name");
            return View(model);
        }

        // POST: Test/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TestViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PayloadResponse response = new PayloadResponse();
                    using (var repository = new WebApiClientRepository<PayloadResponse>())
                    {
                        using (var storage = new LocalStorage())
                        {
                            LoggedInUserInfo userInfo = JsonConvert.DeserializeObject<LoggedInUserInfo>(storage.Get("userInfo").ToString());
                            model.CoachID = userInfo.UserID;
                        }
                        response = repository.GlobalApiCallPost(model, "api/Test/CreateTest");
                        if (response != null)
                        {
                            if (response.Success)
                            {
                                TempData["message_data_success"] = response.Message;
                                return RedirectToAction(nameof(Index));

                            }
                            else
                            {
                                TempData["message_data"] = response.Message;
                                return RedirectToAction(nameof(Index));
                            }
                        }
                        else
                        {
                            TempData["message_data"] = "Problem on Test creation";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                else
                {
                    TempData["message_data"] = "Problem on Test creation";
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch
            {
                TempData["message_data"] = "Problem on Test creation";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAthleteTest(TestAthleteViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PayloadResponse response = new PayloadResponse();
                    using (var repository = new WebApiClientRepository<PayloadResponse>())
                    {

                        response = repository.GlobalApiCallPost(model, "api/Test/CreateTestAthlete");
                        if (response != null)
                        {
                            if (response.Success)
                            {
                                TempData["message_data_success"] = response.Message;
                                return RedirectToAction(nameof(Details), new { id = model.ID });

                            }
                            else
                            {
                                TempData["message_data"] = response.Message;
                                return RedirectToAction(nameof(Details), new { id = model.ID });
                            }
                        }
                    }
                }
                // TODO: Add insert logic here
                TempData["message_data"] = "Problem on Athlete adding";
                return RedirectToAction(nameof(Details), new { id = model.ID });
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditAthleteTest(TestAthleteData data)
        {
            try
            {
                PayloadResponse response = new PayloadResponse();
                TestAthleteListViewModel model = new TestAthleteListViewModel();
                model.ID = Convert.ToInt64(data.TestAthleteID);
                model.Distance = Convert.ToInt32(data.Distance);

                if (ModelState.IsValid)
                {
                    using (var repository = new WebApiClientRepository<PayloadResponse>())
                    {
                        response = repository.GlobalApiCallPost(model, "api/Test/EditTestAthlete");
                        if (response != null)
                        {
                            TempData["message_data_success"] = response.Message;
                            return Ok(new { msg = response.Message });
                        }
                    }
                }
                // TODO: Add insert logic here
                return View();
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Details(long? id)
        {
            TestAthleteViewModel model = new TestAthleteViewModel();
            using (var repository = new WebApiClientRepository<TestAthleteViewModel>())
            {
                var payload = repository.GlobalApiCallGet(null, "api/Test/GetAthleteListByTestID?id=" + id);
                if (payload != null)
                {
                    model = payload;
                }
            }
            ViewBag.AthleteID = new SelectList(GetAthlete(), "ID", "Name");
            return View(model);
        }

        public JsonResult DeleteAthlete(long id)
        {
            try
            {
                if (id > 0)
                {
                    TestAthleteListViewModel model = new TestAthleteListViewModel();
                    model.ID = id;
                    using (var repository = new WebApiClientRepository<TestAthleteListViewModel>())
                    {
                        var payload = repository.GlobalApiCallPost(model, "api/Test/DeleteAthlete");
                        if (payload != null)
                        {

                            return Json(new { Success = true, Message = "Athlete is Deleted successfully" });
                        }
                        return Json(new { Success = true, Message = "Athlete is Deleted successfully" });
                    }

                }
                else
                {
                    return Json(new { Success = true, Message = "Something went wrong !! Please Try later." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = "Athlete can not be deleted now!" });
            }
        }

        public JsonResult DeleteTest(long id)
        {
            try
            {
                if (id > 0)
                {
                    TestAthleteListViewModel model = new TestAthleteListViewModel();
                    model.ID = id;
                    using (var repository = new WebApiClientRepository<TestAthleteListViewModel>())
                    {
                        var payload = repository.GlobalApiCallPost(model, "api/Test/DeleteTest");
                        if (payload != null)
                        {

                            return Json(new { Success = true, Message = "Athlete is Deleted successfully" });
                        }
                        return Json(new { Success = true, Message = "Athlete is Deleted successfully" });
                    }

                }
                else
                {
                    return Json(new { Success = true, Message = "Something went wrong !! Please Try later." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = "Athlete can not be deleted now!" });
            }
        }

        private List<DropDownViewModel> GetTestType()
        {
            List<DropDownViewModel> typeList = new List<DropDownViewModel>();
            using (var repository = new WebApiClientRepository<List<DropDownViewModel>>())
            {
                var payload = repository.GlobalApiCallGet(null, "api/Test/GetTestTypes");
                if (payload != null)
                {
                    typeList = payload.ToList();
                }
            }
            return typeList;
        }

        private List<DropDownViewModel> GetAthlete()
        {
            List<DropDownViewModel> athleteList = new List<DropDownViewModel>();
            using (var repository = new WebApiClientRepository<List<DropDownViewModel>>())
            {
                int userID = 0;
                using (var storage = new LocalStorage())
                {
                    LoggedInUserInfo userInfo = JsonConvert.DeserializeObject<LoggedInUserInfo>(storage.Get("userInfo").ToString());
                    userID = userInfo.UserID;
                }
                var payload = repository.GlobalApiCallGet(null, "api/Test/GetAllAthletesByCoachID?id=" + userID);
                if (payload != null)
                {
                    athleteList = payload.ToList();
                }
            }
            return athleteList;
        }
    }
}