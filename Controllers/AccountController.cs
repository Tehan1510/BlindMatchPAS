using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string Email, string Password)
    {
        // TODO: Replace with real authentication later (ASP.NET Identity)
        if (Email.Contains("@nsbm.ac.lk"))
        {
            // Student
            return RedirectToAction("Index", "Student");
        }
        else if (Email.Contains("supervisor") || Email.Contains("@faculty"))
        {
            // Supervisor
            ViewBag.UserRole = "Supervisor";
            ViewBag.UserName = "Dr. Amara Silva";
            return RedirectToAction("Index", "Supervisor");
        }
        else
        {
            // Admin (for now)
            return RedirectToAction("Index", "Admin");
        }
    }

    public IActionResult Logout()
    {
        return RedirectToAction("Login");
    }
}