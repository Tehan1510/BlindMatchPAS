using BlindMatchPAS.Data;
using BlindMatchPAS.Models;
using BlindMatchPAS.Models.Entities;
using BlindMatchPAS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlindMatchPAS.Controllers
{
    [Authorize(Policy = "SupervisorOnly")]   // <-- ADD THIS LINE
    public class SupervisorController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IMatchService _matchService;
        private readonly AppDbContext _context;

        public SupervisorController(
            IProjectService projectService,
            IMatchService matchService,
            AppDbContext context)
        {
            _projectService = projectService;
            _matchService = matchService;
            _context = context;
        }

        private async Task<SupervisorProfile?> GetCurrentSupervisorAsync()
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            return await _context.SupervisorProfiles
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<IActionResult> Index(string expertise = "All")
        {
            var projects = await _projectService.GetAvailableProjectsAsync(expertise);

            var viewModels = projects.Select(p => new ProjectViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Abstract = p.Abstract,
                ResearchArea = p.ResearchArea,
                TechnicalStack = p.TechnicalStack,
                Status = p.Status
            }).ToList();

            ViewBag.ActiveFilter = expertise;
            ViewBag.SupervisorName = User.FindFirst(ClaimTypes.Name)?.Value;
            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();

            var viewModel = new ProjectViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Abstract = project.Abstract,
                ResearchArea = project.ResearchArea,
                TechnicalStack = project.TechnicalStack,
                Status = project.Status
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmMatch(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();

            var viewModel = new ProjectViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Abstract = project.Abstract,
                ResearchArea = project.ResearchArea,
                TechnicalStack = project.TechnicalStack,
                Status = project.Status
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmMatch(int ProjectId, string ActionType)
        {
            var supervisor = await GetCurrentSupervisorAsync();
            if (supervisor == null) return Unauthorized();

            bool success = await _matchService.ConfirmMatchAsync(ProjectId, supervisor.Id);

            if (success)
            {
                TempData["SuccessMessage"] =
                    $"Project #{ProjectId} successfully matched! " +
                    "The student has been notified and identities are now revealed.";
            }
            else
            {
                TempData["ErrorMessage"] =
                    "Match failed. The project may already be matched, " +
                    "or you have reached your supervision quota.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var supervisor = await GetCurrentSupervisorAsync();
            if (supervisor == null) return NotFound();

            var user = await _context.Users.FindAsync(supervisor.UserId);

            ViewBag.FullName = user?.FullName;
            ViewBag.Email = user?.Email;
            ViewBag.Department = supervisor.Department;
            ViewBag.MaxQuota = supervisor.MaxQuota;
            ViewBag.PreferredAreas = supervisor.PreferredAreas;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(
            string FullName, string Department, int MaxQuota, string[] PreferredAreas)
        {
            var supervisor = await GetCurrentSupervisorAsync();
            if (supervisor == null) return NotFound();

            supervisor.Department = Department;
            supervisor.MaxQuota = MaxQuota;
            supervisor.PreferredAreas = string.Join(",", PreferredAreas ?? Array.Empty<string>());

            var user = await _context.Users.FindAsync(supervisor.UserId);
            if (user != null)
                user.FullName = FullName;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your preferences have been successfully updated.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> MyMatches()
        {
            var supervisor = await GetCurrentSupervisorAsync();
            if (supervisor == null) return NotFound();

            var matches = await _matchService.GetMatchesBySupervisorAsync(supervisor.Id);
            return View(matches);
        }
    }
}