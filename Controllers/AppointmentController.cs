using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimaryVets.Database;
using PrimaryVets.Models;
using System.Security.Cryptography.X509Certificates;

namespace PrimaryVets.Controllers
{
    public class AppointmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Create()
        {
            var schedules = new List<AppointmentSchedule>
            {
                new AppointmentSchedule { TimeSlot = "7 AM" },
                new AppointmentSchedule { TimeSlot = "8 AM" },
                new AppointmentSchedule { TimeSlot = "9 AM" },
                new AppointmentSchedule { TimeSlot = "10 AM" },
                new AppointmentSchedule { TimeSlot = "11 AM" },
                new AppointmentSchedule { TimeSlot = "12 PM" },
                new AppointmentSchedule { TimeSlot = "1 PM" },
                new AppointmentSchedule { TimeSlot = "2 PM" },
                new AppointmentSchedule { TimeSlot = "3 PM" },
                new AppointmentSchedule { TimeSlot = "4 PM" },
                new AppointmentSchedule { TimeSlot = "5 PM" },
                new AppointmentSchedule { TimeSlot = "6 PM" },
                new AppointmentSchedule { TimeSlot = "7 PM" },
                new AppointmentSchedule { TimeSlot = "8 PM" },
                new AppointmentSchedule { TimeSlot = "9 PM" },
                new AppointmentSchedule { TimeSlot = "10 PM" },
                new AppointmentSchedule { TimeSlot = "11 PM" }
            };

            return View(schedules);
        }

        [HttpPost]
        public IActionResult Create(List<AppointmentSchedule> model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }
            return View(model);
        }
    }
}
