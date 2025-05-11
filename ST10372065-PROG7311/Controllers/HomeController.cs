using System.Diagnostics;
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
        public IActionResult Login(string email, string password)
        {
            // Add your login logic here
            if (email == "test@example.com" && password == "password")
            {
                // Redirect or return success
                return RedirectToAction("HomePage");
            }

            // Return an error message or reload the modal
            ViewBag.ErrorMessage = "Invalid email or password.";
            return View();
        }


        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
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