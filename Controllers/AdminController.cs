using BlindMatchPAS.Data;
using BlindMatchPAS.Models;
using BlindMatchPAS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlindMatchPAS.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMatchService _matchService;

        // In-memory research areas (can be moved to DB later)
        private static List<string> _researchAreas = new List<string>
        {
            "AI", "Software", "Java", "C#", "Networking", "Data Science", "Cyber Security"
        };

        public AdminController(AppDbContext context, IMatchService matchService)
        {
            _context = context;
            _matchService = matchService;
        }

        // GET: /Admin/Admin - Main Dashboard
        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            var matches = await _matchService.GetAllMatchesAsync();

            var viewModels = matches.Select(m => new AdminMatchViewModel
            {
                StudentName = m.Project.StudentProfile.User.FullName,
                ProjectTitle = m.Project.Title,
                SupervisorName = m.SupervisorProfile.User.FullName,
                ResearchArea = m.Project.ResearchArea,
                Status = m.Status,
                MatchedAt = m.MatchedAt
            }).ToList();

            ViewBag.CurrentTags = _researchAreas;
            return View(viewModels);
        }

        // GET: /Admin/Index - redirects to Admin dashboard
        [HttpGet]
        public IActionResult Index() => RedirectToAction(nameof(Admin));

        // GET: /Admin/Matches
        [HttpGet]
        public async Task<IActionResult> Matches()
        {
            var matches = await _matchService.GetAllMatchesAsync();

            var viewModels = matches.Select(m => new AdminMatchViewModel
            {
                StudentName = m.Project.StudentProfile.User.FullName,
                ProjectTitle = m.Project.Title,
                SupervisorName = m.SupervisorProfile.User.FullName,
                ResearchArea = m.Project.ResearchArea,
                Status = m.Status,
                MatchedAt = m.MatchedAt
            }).ToList();

            return View(viewModels);
        }

        // GET: /Admin/Students
        [HttpGet]
        public async Task<IActionResult> Students()
        {
            var students = await _context.Users
                .Where(u => u.Role == "Student")
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new UserManagementViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return View(students);
        }

        // GET: /Admin/Supervisors
        [HttpGet]
        public async Task<IActionResult> Supervisors()
        {
            var supervisors = await _context.Users
                .Where(u => u.Role == "Supervisor")
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new UserManagementViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return View(supervisors);
        }

        // GET: /Admin/Research
        [HttpGet]
        public IActionResult Research()
        {
            ViewBag.CurrentTags = _researchAreas;
            return View();
        }

        // POST: /Admin/AddResearchArea
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddResearchArea(string NewArea)
        {
            if (!string.IsNullOrWhiteSpace(NewArea) && !_researchAreas.Contains(NewArea))
            {
                _researchAreas.Add(NewArea.Trim());
                TempData["SuccessMessage"] = $"Research area '{NewArea}' added successfully.";
            }
            return RedirectToAction(nameof(Research));
        }

        // POST: /Admin/RemoveResearchArea
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveResearchArea(string areaName)
        {
            if (_researchAreas.Contains(areaName))
            {
                _researchAreas.Remove(areaName);
                TempData["SuccessMessage"] = $"Research area '{areaName}' removed.";
            }
            return RedirectToAction(nameof(Research));
        }

        // GET: /Admin/Settings
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            ViewBag.TotalStudents = await _context.Users.CountAsync(u => u.Role == "Student");
            ViewBag.TotalSupervisors = await _context.Users.CountAsync(u => u.Role == "Supervisor");
            ViewBag.TotalMatches = await _context.Matches.CountAsync();
            ViewBag.TotalProjects = await _context.Projects.CountAsync();
            ViewBag.PendingProjects = await _context.Projects.CountAsync(p => p.Status == "Pending");
            ViewBag.MatchedProjects = await _context.Projects.CountAsync(p => p.Status == "Matched");
            return View();
        }

        // POST: /Admin/DeleteUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction(nameof(Admin));
            }

            string role = user.Role;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"User '{user.FullName}' has been removed.";

            return role == "Student"
                ? RedirectToAction(nameof(Students))
                : RedirectToAction(nameof(Supervisors));
        }

        // Public static method so other controllers can access research areas
        public static List<string> GetResearchAreas() => _researchAreas;
    }
}