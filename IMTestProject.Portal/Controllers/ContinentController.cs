using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMTestProject.Common;
using IMTestProject.Common.Dto;
using IMTestProject.Common.Dto.Continent;
using IMTestProject.Common.Dto.TableConfiguration;
using IMTestProject.Portal.Models;
using IMTestProject.Portal.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IMTestProject.Portal.Controllers
{
    public class ContinentController : Controller
    {
        // GET: Continent

        public IActionResult Index()
        {
            List<ContinentDto> continentList = new List<ContinentDto>();
            using (var repository = new WebApiClientRepository<List<ContinentDto>>())
            {
                var payload = repository.GetList(null, "Continent/GetList");
                if (payload != null)
                {
                    if (payload.data != null)
                    {
                        continentList = JsonConvert.DeserializeObject<List<ContinentDto>>(payload.data.ToString());
                    }
                }
            }
            return View(continentList.ToList());
        }

        // GET: Continent/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Continent/Create
        public ActionResult Create()
        {
            AddContinentDto continent = new AddContinentDto();
            using (var repository = new WebApiClientRepository<TableConfigurationDto>())
            {
                var payload = repository.GetList(null, "TableConfiguration/GetList");
                if (payload != null)
                {
                    if (payload.data != null)
                    {
                        continent.TableConfigurationDtos = JsonConvert.DeserializeObject<List<TableConfigurationDto>>(payload.data.ToString());
                    }
                }
            }

            return View(continent);
        }

        // POST: Continent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddContinentDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContinentDto continent = new ContinentDto();
                    using (var repository = new WebApiClientRepository<ContinentDto>())
                    {
                        var response = repository.Post(model, "Continent");
                        if (response != null)
                        {
                            if (response.data != null)
                            {
                                continent = JsonConvert.DeserializeObject<ContinentDto>(response.data.ToString());
                                TempData["message_data"] = response.message;
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                TempData["message_data"] = response.message;
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
                // TODO: Add insert logic here
                TempData["message_data"] = "Problem on Athlete adding";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Continent/Edit/5
        public ActionResult Edit(int id)
        {
            EditContinentDto continent = new EditContinentDto();
            using (var repository = new WebApiClientRepository<EditContinentDto>())
            {
                var payload = repository.GetById(id, "Continent/");
                if (payload != null)
                {
                    if (payload.data != null)
                    {
                        continent = JsonConvert.DeserializeObject<EditContinentDto>(payload.data.ToString());
                    }
                }
            }
            return View(continent);
        }

        // POST: Continent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditContinentDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContinentDto continent = new ContinentDto();
                    using (var repository = new WebApiClientRepository<ContinentDto>())
                    {
                        var response = repository.Put(model, "Continent");
                        if (response != null)
                        {
                            if (response.data != null)
                            {
                                continent = JsonConvert.DeserializeObject<ContinentDto>(response.data.ToString());
                                TempData["message_data"] = response.message;
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                TempData["message_data"] = response.message;
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
                // TODO: Add insert logic here
                TempData["message_data"] = "Problem on Athlete adding";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Continent/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Continent/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}