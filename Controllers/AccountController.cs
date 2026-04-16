using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BlindMatchPAS.Controllers
{
    public class AccountController : Controller
    {
        private readonly Dictionary<string, string> _testUsers = new()
        {
            { "student@student.ac.nsbm.lk", "student123" },
            { "supervisor@superviouse.ac.nsbm.lk", "super123" },
            { "admin@admin.ac.nsbm.lk", "admin123" }
        };

        [HttpGet] public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            string loginEmail = Email?.ToLower();
            if (_testUsers.ContainsKey(loginEmail) && _testUsers[loginEmail] == Password)
            {
                if (loginEmail.EndsWith("@admin.ac.nsbm.lk")) return RedirectToAction("Admin", "Admin");

                // FIXED: Now points to the specific StudentDashboard action
                if (loginEmail.EndsWith("@student.ac.nsbm.lk")) return RedirectToAction("StudentDashboard", "Student");

                return RedirectToAction("Index", "Supervisor");
            }
            ModelState.AddModelError("", "Invalid credentials.");
            return View();
        }

        [HttpPost]
        public IActionResult UploadProposal(ProposalUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                return RedirectToAction("SubmitProject", "Student");
            }
            return View(model);
        }

    }
}