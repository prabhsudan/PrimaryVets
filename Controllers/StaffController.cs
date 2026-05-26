using Microsoft.AspNetCore.Mvc;

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
    }
}
