using Microsoft.AspNetCore.Mvc;

namespace PrimaryVets.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
