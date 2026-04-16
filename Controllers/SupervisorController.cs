using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BlindMatchPAS.Controllers
{
    public class SupervisorController : Controller
    {
        private List<ProjectViewModel> GetDummyProjects()
        {
            return new List<ProjectViewModel>
            {
                new ProjectViewModel { Id = 1, Title = "AI Traffic Optimization", Abstract = "Utilizes neural networks to predict urban traffic flow.", ResearchArea = "AI", Status = "Pending", Date = "2026-04-10" },
                new ProjectViewModel { Id = 2, Title = "Hotel Management System", Abstract = "Desktop application for booking and room management.", ResearchArea = "Java", Status = "Pending", Date = "2026-04-12" },
                new ProjectViewModel { Id = 3, Title = "E-Commerce Web Portal", Abstract = "Scalable software architecture for online retail.", ResearchArea = "Software", Status = "Pending", Date = "2026-04-14" },
                new ProjectViewModel { Id = 4, Title = "Secure Banking API", Abstract = "Enterprise-level API for handling secure transactions.", ResearchArea = "C#", Status = "Pending", Date = "2026-04-15" }
            };
        }

        public IActionResult Index(string expertise = "All")
        {
            var allProjects = GetDummyProjects();
            var filteredProjects = expertise == "All" ? allProjects : allProjects.Where(p => p.ResearchArea == expertise).ToList();

            ViewBag.ActiveFilter = expertise;
            return View(filteredProjects);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var project = GetDummyProjects().FirstOrDefault(p => p.Id == id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost]
        public IActionResult ConfirmMatch(int ProjectId, string ActionType)
        {
            TempData["SuccessMessage"] = $"Project #{ProjectId} successfully accepted!";
            return RedirectToAction("Index");
        }

        public IActionResult Settings() => View();
    }
}