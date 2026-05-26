using Microsoft.AspNetCore.Mvc;
using PrimaryVets.Interface;

namespace PrimaryVets.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Username and Password are required";
                return View();
            }

            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            if (!user.IsActive)
            {
                ViewBag.Error = "User account is disabled";
                return View();
            }

            if (user.Password != password)
            {
                ViewBag.Error = "Invalid password";
                return View();
            }

            HttpContext.Session.SetString("Email", user.Username);
            HttpContext.Session.SetString("UserRole", user.Role ?? "");
            HttpContext.Session.SetString("UserId", user.Id.ToString());

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}
