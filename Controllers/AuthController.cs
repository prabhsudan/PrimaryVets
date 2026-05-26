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
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }
            else if (string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Password is required";
                return View();
            }
            else if (user.Password != password)
            {
                ViewBag.Error = "Invalid Password";
                return View();
            }
            else if (user == null || !user.IsActive)
            {
                ViewBag.Error = "User not found or account is disabled";
                return View();
            }

            else
            {
                HttpContext.Session.SetString("Email", user.Username);
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserId", user.Id.ToString());
            }
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}
