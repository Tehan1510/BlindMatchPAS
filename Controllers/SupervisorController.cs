using Microsoft.AspNetCore.Mvc;
using BlindMatchPAS.Models;
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
                new ProjectViewModel { Id = 1, Title = "AI Traffic Optimization", Abstract = "Utilizes neural networks to predict urban traffic flow.", ResearchArea = "AI" },
                new ProjectViewModel { Id = 2, Title = "Hotel Management System", Abstract = "Desktop application for booking and room management.", ResearchArea = "Java" },
                new ProjectViewModel { Id = 3, Title = "E-Commerce Web Portal", Abstract = "Scalable software architecture for online retail.", ResearchArea = "Software" },
                new ProjectViewModel { Id = 4, Title = "Secure Banking API", Abstract = "Enterprise-level API for handling secure transactions.", ResearchArea = "C#" }
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

        [HttpGet]
        public IActionResult ConfirmMatch(int id)
        {
            var project = GetDummyProjects().FirstOrDefault(p => p.Id == id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost]
        public IActionResult ConfirmMatch(int ProjectId, string ActionType)
        {
            TempData["SuccessMessage"] = $"Project #{ProjectId} successfully accepted! The student has been notified and identities are now revealed.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Settings(string action)
        {
            TempData["SuccessMessage"] = "Your preferences have been successfully updated.";
            return RedirectToAction("Index");
        }
    }
}