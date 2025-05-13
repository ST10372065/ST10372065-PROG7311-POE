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
            var user = await _userService.ValidateUserAsync(email, password);

            if (user != null)
            {
                // Return success response
                return Json(new { success = true, redirectUrl = Url.Action("Products", "Home") });
            }

            // Return error response
            return Json(new { success = false, errorMessage = "Invalid email or password." });
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

            var products = await _userService.GetAllProductsAsync(); // Fetch all products
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