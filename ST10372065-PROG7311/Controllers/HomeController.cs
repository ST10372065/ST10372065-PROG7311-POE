using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ST10372065_PROG7311.Models;
using ST10372065_PROG7311.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.AddAsync(user);
                return RedirectToAction("HomePage");
            }
            // Log validation errors
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    _logger.LogError("ModelState error for {Key}: {ErrorMessage}", key, error.ErrorMessage);
                }
            }
            return RedirectToAction("HomePage");
        }



        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Log the login attempt
            _logger.LogInformation("Login attempt with email: {email}", email);
            // Validate the user credentials
            var user = await _userService.ValidateUserAsync(email, password);

            if (user != null)
            {
                _logger.LogInformation("Login successful for email: {email}", email);

                // Create claims for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    // Store UserId for later use
                    new Claim("UserId", user.UserId.ToString()),
                    // Store the user's first name
                    new Claim("FirstName", user.FirstName), 
                    // Store the user's role
                    new Claim(ClaimTypes.Role, user.Role) 
                };

                // Create the identity and principal
                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user and set the authentication cookie
                await HttpContext.SignInAsync("Cookies", principal);

                // Return success response
                return Json(new { success = true });
            }
            /// Log the failed login attempt
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
        public async Task<IActionResult> Products(int? farmerId = null, string category = null, DateOnly? startDate = null, DateOnly? endDate = null)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            // Get the current user
            var user = await _userService.GetByEmailAsync(User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<Product> products;

            // If the user is a Farmer, only show their products (ignore filters)
            if (User.IsInRole("Farmer"))
            {
                products = await _userService.GetProductsByUserIdAsync(user.UserId);
            }
            // If the user is an Employee, apply filters
            else
            {
                products = await _userService.FilterProductsAsync(farmerId, category, startDate, endDate);

                // Pass all farmers to the view for the dropdown
                ViewBag.Farmers = await _userService.GetAllFarmersAsync();

                // Get all distinct categories for the dropdown
                var allProducts = await _userService.GetAllProductsAsync();
                ViewBag.AllCategories = allProducts
                    .Select(p => p.Category)
                    .Distinct()
                    .ToList();

                // Pass the filter values to the view to maintain state
                ViewBag.SelectedFarmerId = farmerId;
                ViewBag.SelectedCategory = category;
                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;
            }

            ViewBag.Products = products;

            return View();
        }

        [Authorize(Roles = "Farmer")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(
        [FromForm] string ProductName,
        [FromForm] string Category,
        [FromForm] decimal Price,
        [FromForm] DateOnly Date,
        [FromForm] IFormFile Image)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                var user = await _userService.GetByEmailAsync(User.Identity.Name);

                // Save the image to wwwroot/images/products
                string imageUrl = null;
                if (Image != null && Image.Length > 0)
                {
                    // Create the directory if it doesn't exist
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                    Directory.CreateDirectory(uploads);
                    // Generate a unique file name
                    var fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                    // Combine the path and file name
                    var filePath = Path.Combine(uploads, fileName);
                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }
                    // Set the image URL to be stored in the database
                    imageUrl = "/images/products/" + fileName;
                }

                var product = new Product
                {
                    ProductName = ProductName,
                    Category = Category,
                    Price = Price,
                    Date = Date,
                    ImageUrl = imageUrl,
                    UserId = user.UserId
                };
                // Add the product to the database
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