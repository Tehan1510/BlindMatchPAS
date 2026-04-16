using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BlindMatchPAS.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Index() => RedirectToAction(nameof(Admin));

        // 1. ADMIN DASHBOARD
        public static List<string> CurrentTags { get; set; } = new List<string> { "AI", "Machine Learning", "Data Science", "Cyber Security" };

        [HttpGet]
        public IActionResult Admin()
        {
            ViewBag.CurrentTags = CurrentTags;
            var summaryMatches = new List<AdminMatchViewModel>
            {
                new AdminMatchViewModel { Student1 = "Alice Johnson", Student2 = "Bob Smith", Supervisor = "Dr. Aruna Pathirana", Status = "Approved" },
                new AdminMatchViewModel { Student1 = "Sadeesa Damruwan", Student2 = "Charlie Brown", Supervisor = "Prof. Samantha Perera", Status = "Pending" }
            };
            return View(summaryMatches);
        }

        // 2. STUDENT DASHBOARD
        [HttpGet]
        public IActionResult StudentDashboard()
        {
            var myProposals = new List<ProjectViewModel>
            {
                new ProjectViewModel { Title = "AI-Powered Fashion App", Status = "Matched", Date = "2026-03-15" },
                new ProjectViewModel { Title = "Smart Emergency Alert", Status = "Pending", Date = "2026-04-10" }
            };
            return View(myProposals);
        }

        [HttpPost]
        public IActionResult AddArea(string NewArea)
        {
            if (!string.IsNullOrWhiteSpace(NewArea) && !CurrentTags.Contains(NewArea)) CurrentTags.Add(NewArea);
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult RemoveArea(string areaName)
        {
            if (CurrentTags.Contains(areaName)) CurrentTags.Remove(areaName);
            return RedirectToAction("Admin");
        }
    }

    // --- CONSOLIDATED DATA MODELS ---

    public class ProjectViewModel
    {
        // Combined properties for both Student & Supervisor dashboards
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string Abstract { get; set; }
        public string ResearchArea { get; set; }
    }

    public class AdminMatchViewModel
    {
        public string Student1 { get; set; }
        public string Student2 { get; set; }
        public string Supervisor { get; set; }
        public string Status { get; set; }
    }

    public class ProjectMatchViewModel
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public double GPA { get; set; }
        public string SupervisorName { get; set; }
        public string ResearchArea { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
    }

    public class ProposalUploadViewModel
    {
        public string ProjectTitle { get; set; }
        public string Category { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile ProjectFile { get; set; }
    }

    public class ResearchTopicViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Field { get; set; }
        public string SupervisorName { get; set; }
        public string Summary { get; set; }
        public string Requirements { get; set; }
    }
}