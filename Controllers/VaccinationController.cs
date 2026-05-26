using Microsoft.AspNetCore.Mvc;
using PrimaryVets.Models;

namespace PrimaryVets.Controllers
{
    public class VaccinationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult VaccinationList()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(VaccinationModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(VaccinationModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
