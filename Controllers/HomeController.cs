using Microsoft.AspNetCore.Mvc;

namespace BlindMatchPAS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Splash()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}