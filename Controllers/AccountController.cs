using BlindMatchPAS.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace BlindMatchPAS.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in redirect to their dashboard
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToDashboard();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError(string.Empty, "Email and Password are required.");
                return View();
            }

            var user = await _authService.ValidateUserAsync(Email, Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }

            // Build claims for the cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            // IsPersistent = false means cookie deleted when browser closes
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });

            return RedirectToDashboard(user.Role);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            string FullName, string Email, string Password, string ConfirmPassword)
        {
            // Validate password match
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View();
            }

            // Validate password length
            if (Password.Length < 6)
            {
                ModelState.AddModelError(string.Empty,
                    "Password must be at least 6 characters.");
                return View();
            }

            // Validate email format using Regex
            var emailRegex = new Regex(
                @"^[a-zA-Z0-9._%+-]+@(student|supervisor|admin)\.nsbm\.ac\.lk$",
                RegexOptions.IgnoreCase);

            if (!emailRegex.IsMatch(Email))
            {
                ModelState.AddModelError(string.Empty,
                    "Please use a valid NSBM university email address.");
                return View();
            }

            // Check if email already exists
            if (await _authService.EmailExistsAsync(Email))
            {
                ModelState.AddModelError(string.Empty,
                    "An account with this email already exists.");
                return View();
            }

            // Determine role from email domain
            string role = _authService.DetermineRole(Email);

            if (string.IsNullOrEmpty(role))
            {
                ModelState.AddModelError(string.Empty,
                    "Could not determine role from email domain.");
                return View();
            }

            // Register the user
            bool success = await _authService.RegisterUserAsync(
                FullName, Email, Password, role);

            if (!success)
            {
                ModelState.AddModelError(string.Empty,
                    "Registration failed. Please try again.");
                return View();
            }

            TempData["SuccessMessage"] = "Account created successfully. Please log in.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Helper to redirect user to their correct dashboard based on role
        private IActionResult RedirectToDashboard(string? role = null)
        {
            role ??= User.FindFirst(ClaimTypes.Role)?.Value;

            return role switch
            {
                "Student" => RedirectToAction("Index", "Student"),
                "Supervisor" => RedirectToAction("Index", "Supervisor"),
                "Admin" => RedirectToAction("Index", "Admin"),
                _ => RedirectToAction("Index", "Home")
            };
        }
    }
}