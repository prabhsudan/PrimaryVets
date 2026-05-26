using Microsoft.AspNetCore.Mvc;
using PrimaryVets.Models;

namespace PrimaryVets.Controllers
{
    public class ReminderController : Controller
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
        public IActionResult Create(RemainderViewModel model)
        {
            if (model.Date < DateTime.Today)
            {
                ModelState.AddModelError("Date", "Date cannot be in the past.");
            }

            if (!ModelState.IsValid)
                return View(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Edit (RemainderViewModel model)
        {
            if (model.Date < DateTime.Today)
            {
                ModelState.AddModelError("Date", "Date cannot be in the past.");
            }

            if (!ModelState.IsValid)
                return View(model);

            return RedirectToAction("Index");
        }
    }
}
