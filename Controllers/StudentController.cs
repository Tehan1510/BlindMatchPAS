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
    [Authorize(Policy = "StudentOnly")]
    public class StudentController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly AppDbContext _context;

        public StudentController(IProjectService projectService, AppDbContext context)
        {
            _projectService = projectService;
            _context = context;
        }

        private async Task<StudentProfile?> GetCurrentStudentAsync()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        // GET: /Student/Index - Student Dashboard
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var student = await GetCurrentStudentAsync();
            if (student == null) return NotFound();

            var projects = await _projectService.GetProjectsByStudentAsync(student.Id);

            var viewModels = projects.Select(p => new ProjectViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Abstract = p.Abstract,
                ResearchArea = p.ResearchArea,
                TechnicalStack = p.TechnicalStack,
                Status = p.Status,
                SubmittedAt = p.SubmittedAt,
                // Only reveal supervisor info if matched
                SupervisorName = (p.Match != null && p.Match.IdentityRevealed)
                    ? p.Match.SupervisorProfile.User.FullName : null,
                SupervisorEmail = (p.Match != null && p.Match.IdentityRevealed)
                    ? p.Match.SupervisorProfile.User.Email : null
            }).ToList();

            ViewBag.StudentName = User.Identity?.Name;
            return View(viewModels);
        }

        // GET: /Student/SubmitProject
        [HttpGet]
        public IActionResult SubmitProject()
        {
            ViewBag.ResearchAreas = AdminController.GetResearchAreas();
            return View();
        }

        // POST: /Student/SubmitProject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitProject(string Title, string Abstract,
            string TechnicalStack, string ResearchArea)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Abstract)
                || string.IsNullOrWhiteSpace(TechnicalStack) || string.IsNullOrWhiteSpace(ResearchArea))
            {
                ModelState.AddModelError(string.Empty, "All fields are required.");
                ViewBag.ResearchAreas = AdminController.GetResearchAreas();
                return View();
            }

            var student = await GetCurrentStudentAsync();
            if (student == null) return NotFound();

            var project = new Project
            {
                Title = Title.Trim(),
                Abstract = Abstract.Trim(),
                TechnicalStack = TechnicalStack.Trim(),
                ResearchArea = ResearchArea.Trim(),
                Status = "Pending",
                SubmittedAt = DateTime.UtcNow,
                StudentProfileId = student.Id
            };

            bool success = await _projectService.SubmitProjectAsync(project);

            if (success)
            {
                TempData["SuccessMessage"] = "Your project proposal has been submitted successfully!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Failed to submit project. Please try again.");
            ViewBag.ResearchAreas = AdminController.GetResearchAreas();
            return View();
        }

        // GET: /Student/EditProject/5
        [HttpGet]
        public async Task<IActionResult> EditProject(int id)
        {
            var student = await GetCurrentStudentAsync();
            if (student == null) return NotFound();

            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null || project.StudentProfileId != student.Id)
                return NotFound();

            if (project.Status == "Matched" || project.Status == "Withdrawn")
            {
                TempData["ErrorMessage"] = "You cannot edit a matched or withdrawn project.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new ProjectViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Abstract = project.Abstract,
                ResearchArea = project.ResearchArea,
                TechnicalStack = project.TechnicalStack,
                Status = project.Status,
                SubmittedAt = project.SubmittedAt
            };

            ViewBag.ResearchAreas = AdminController.GetResearchAreas();
            return View(viewModel);
        }

        // POST: /Student/EditProject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(int Id, string Title, string Abstract,
            string TechnicalStack, string ResearchArea)
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Abstract)
                || string.IsNullOrWhiteSpace(TechnicalStack) || string.IsNullOrWhiteSpace(ResearchArea))
            {
                ModelState.AddModelError(string.Empty, "All fields are required.");
                ViewBag.ResearchAreas = AdminController.GetResearchAreas();

                var existingProject = await _projectService.GetProjectByIdAsync(Id);
                if (existingProject == null) return NotFound();

                return View(new ProjectViewModel
                {
                    Id = existingProject.Id,
                    Title = existingProject.Title,
                    Abstract = existingProject.Abstract,
                    ResearchArea = existingProject.ResearchArea,
                    TechnicalStack = existingProject.TechnicalStack,
                    Status = existingProject.Status
                });
            }

            var student = await GetCurrentStudentAsync();
            if (student == null) return NotFound();

            var updateProject = new Project
            {
                Id = Id,
                Title = Title.Trim(),
                Abstract = Abstract.Trim(),
                TechnicalStack = TechnicalStack.Trim(),
                ResearchArea = ResearchArea.Trim()
            };

            bool success = await _projectService.UpdateProjectAsync(updateProject);

            if (success)
            {
                TempData["SuccessMessage"] = "Project updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to update. Project may already be matched.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Student/WithdrawProject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WithdrawProject(int id)
        {
            var student = await GetCurrentStudentAsync();
            if (student == null) return NotFound();

            bool success = await _projectService.WithdrawProjectAsync(id, student.Id);

            if (success)
            {
                TempData["SuccessMessage"] = "Project withdrawn successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Cannot withdraw this project. It may already be matched.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}