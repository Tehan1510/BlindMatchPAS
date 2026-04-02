using Microsoft.AspNetCore.Mvc;
using BlindMatchPAS.Models;

public class SupervisorController : Controller
{
    private static List<ProjectViewModel> _projects = new List<ProjectViewModel>
    {
        new ProjectViewModel 
        { 
            Id = 1, 
            Title = "AI-Powered Exam Proctoring System", 
            Abstract = "Real-time cheating detection using computer vision and deep learning...", 
            ResearchArea = "Artificial Intelligence", 
            TechStack = "Python, OpenCV, TensorFlow" 
        },
        new ProjectViewModel 
        { 
            Id = 2, 
            Title = "Secure Blockchain Voting Platform", 
            Abstract = "Decentralized voting system with end-to-end encryption...", 
            ResearchArea = "Cybersecurity", 
            TechStack = "Solidity, Ethereum, React" 
        }
    };

    public IActionResult Index()
    {
        ViewBag.UserRole = "Supervisor";
        ViewBag.UserName = "Dr. Amara Silva";
        return View(_projects);
    }

    // GET - Show confirmation page
    public IActionResult ConfirmMatch(int id)
    {
        var project = _projects.FirstOrDefault(p => p.Id == id);
        if (project == null) return NotFound();
        return View(project);
    }

    // POST - Process the confirmation
    [HttpPost]
    public IActionResult Confirm(int id)          // ← Different action name to avoid conflict
    {
        TempData["Success"] = "✅ Match Confirmed! Identity has been revealed to both parties.";
        return RedirectToAction("Reveal", new { id = id });
    }

    public IActionResult Reveal(int id)
    {
        ViewBag.UserRole = "Supervisor";
        ViewBag.UserName = "Dr. Amara Silva";
        var project = _projects.FirstOrDefault(p => p.Id == id);
        return View(project);
    }
}