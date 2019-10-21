using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMTestProject.Common.Dto.TableConfiguration;
using IMTestProject.Portal.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IMTestProject.Portal.Controllers
{
    public class TableConfigurationController : Controller
    {
        // GET: TableConfiguration
        public IActionResult Index()
        {
            List<TableConfigurationDto> configurationList = new List<TableConfigurationDto>();
            using (var repository = new WebApiClientRepository<List<TableConfigurationDto>>())
            {
                var payload = repository.GetList(null, "TableConfiguration/GetList");
                if (payload != null)
                {
                    if (payload.data != null)
                    {
                        configurationList = JsonConvert.DeserializeObject<List<TableConfigurationDto>>(payload.data.ToString());
                    }
                }
            }
            return View(configurationList.ToList());
        }

        // GET: TableConfiguration/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TableConfiguration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TableConfiguration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddTableConfigurationDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TableConfigurationDto tblConfiguration = new TableConfigurationDto();
                    using (var repository = new WebApiClientRepository<TableConfigurationDto>())
                    {
                        var response = repository.Post(model, "TableConfiguration");
                        if (response != null)
                        {
                            if (response.data != null)
                            {
                                tblConfiguration = JsonConvert.DeserializeObject<TableConfigurationDto>(response.data.ToString());
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

        // GET: TableConfiguration/Edit/5
        public ActionResult Edit(int id)
        {
            EditTableConfigurationDto continent = new EditTableConfigurationDto();
            using (var repository = new WebApiClientRepository<EditTableConfigurationDto>())
            {
                var payload = repository.GetById(id, "TableConfiguration/");
                if (payload != null)
                {
                    if (payload.data != null)
                    {
                        continent = JsonConvert.DeserializeObject<EditTableConfigurationDto>(payload.data.ToString());
                    }
                }
            }
            return View(continent);
        }

        // POST: TableConfiguration/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditTableConfigurationDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TableConfigurationDto continent = new TableConfigurationDto();
                    using (var repository = new WebApiClientRepository<TableConfigurationDto>())
                    {
                        var response = repository.Put(model, "TableConfiguration");
                        if (response != null)
                        {
                            if (response.data != null)
                            {
                                continent = JsonConvert.DeserializeObject<TableConfigurationDto>(response.data.ToString());
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

        // GET: TableConfiguration/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TableConfiguration/Delete/5
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