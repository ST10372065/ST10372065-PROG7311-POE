using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ST10372065_PROG7311.Models;
using ST10372065_PROG7311.Services;

namespace ST10372065_PROG7311.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserService _userService;

        public HomeController(ILogger<HomeController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.AddAsync(user);
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            _logger.LogInformation("Login attempt with email: {email}", email);

            var user = await _userService.ValidateUserAsync(email, password);

            if (user != null)
            {
                _logger.LogInformation("Login successful for email: {email}", email);

                // Create claims for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("UserId", user.UserId.ToString()), // Store UserId for later use
                    new Claim("FirstName", user.FirstName), // Store the user's first name
                    new Claim(ClaimTypes.Role, user.Role) // Store the user's role
                };

                // Create the identity and principal
                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user and set the authentication cookie
                await HttpContext.SignInAsync("Cookies", principal);

                // Return success response
                return Json(new { success = true });
            }

            _logger.LogWarning("Login failed for email: {email}", email);

            // Return error response
            return Json(new { success = false, errorMessage = "Invalid email or password." });
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("HomePage");
        }

        public IActionResult HomePage()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Products()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            // Get the logged-in user's ID from the authentication cookie
            var userId = int.Parse(User.FindFirst("UserId").Value);

            // Fetch products for the logged-in user
            var products = await _userService.GetProductsByUserIdAsync(userId);

            ViewBag.Products = products;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                var user = await _userService.GetByEmailAsync(User.Identity.Name); // Get the logged-in user
                product.UserId = user.UserId;
                await _userService.AddProductAsync(product);
                return RedirectToAction("Products");
            }

            return View("Products");
        }

        public async Task<IActionResult> Privacy()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}