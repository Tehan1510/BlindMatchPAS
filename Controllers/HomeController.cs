using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlindMatchPAS.Models;

namespace BlindMatchPAS.Controllers;

public class HomeController : Controller
{
public IActionResult Index()
{
    // Create some temporary dummy data to test the UI
    var dummyProjects = new List<ProjectViewModel>
    {
        new ProjectViewModel 
        { 
            Title = "AI Matchmaking System", 
            Abstract = "An exploratory study into AI.", 
            ResearchArea = "Machine Learning",
            TechStack = "Python, TensorFlow" // Assuming these properties exist based on your view
        },
        new ProjectViewModel 
        { 
            Title = "Secure Web Portal", 
            Abstract = "A new standard for web security.", 
            ResearchArea = "Cybersecurity",
            TechStack = "C#, ASP.NET"
        }
    };

    // Pass the dummy data to the view
    return View(dummyProjects); 
}

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
