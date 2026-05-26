using Microsoft.AspNetCore.Mvc;
using PrimaryVets.Models;

namespace PrimaryVets.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StaffVm model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(StaffVm model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Edit");
            }
            return View(model);
        }
    }
}
