using Microsoft.AspNetCore.Mvc;

namespace BlindMatchPAS.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError(string.Empty, "Email and Password are required.");
                return View();
            }

            // HARDCODED TEST: Remove before coursework submission!
            if (Password != "12345")
            {
                ModelState.AddModelError(string.Empty, "Invalid password. (Use '12345' for testing)");
                return View();
            }

            string loginEmail = Email.ToLower();

            if (loginEmail.EndsWith("@student.ac.nsbm.lk")) return RedirectToAction("Index", "Student");
            if (loginEmail.EndsWith("@superviouse.ac.nsbm.lk") || loginEmail.Contains("supervisor")) return RedirectToAction("Index", "Supervisor");
            if (loginEmail.EndsWith("@admin.ac.nsbm.lk")) return RedirectToAction("Index", "Admin");

            ModelState.AddModelError(string.Empty, "Invalid university email domain.");
            return View();
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string FullName, string Email, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View();
            }
            return RedirectToAction("Login");
        }
    }
}