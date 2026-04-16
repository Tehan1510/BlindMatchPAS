using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BlindMatchPAS.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet]
        public IActionResult StudentDashboard()
        {
            // Dummy data for your NSBM presentation
            var myProposals = new List<ProjectViewModel>
            {
                new ProjectViewModel { Title = "AI-Powered Fashion App", Status = "Matched", Date = "2026-03-15" },
                new ProjectViewModel { Title = "Smart Emergency Alert", Status = "Pending", Date = "2026-04-10" }
            };

            return View(myProposals);
        }

        // I noticed you have a SubmitProject.cshtml view! This will make it work.
        [HttpGet]
        public IActionResult SubmitProject()
        {
            return View();
        }
    }
}