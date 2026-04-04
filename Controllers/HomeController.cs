using Microsoft.AspNetCore.Mvc;

namespace BlindMatchPAS.Controllers
{
    public class HomeController : Controller
    {
        // NEW: This serves the Splash screen
        public IActionResult Splash()
        {
            return View();
        }

        // This is your Welcome screen
        public IActionResult Index()
        {
            return View();
        }
    }
}