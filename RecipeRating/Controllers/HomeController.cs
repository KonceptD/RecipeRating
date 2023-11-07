using Microsoft.AspNetCore.Mvc;
using RecipeRating.Models;
using System.Diagnostics;

namespace RecipeRating.Controllers
{
    // The HomeController handles requests related to the home page and privacy policy of the application.
    public class HomeController : Controller
    {
        // The logger is used to log information, warnings, and errors during the execution of the application.
        private readonly ILogger<HomeController> _logger;

        // The constructor injects an ILogger instance specific to the HomeController for logging purposes.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // This action method returns the default view for the home page of the application.
        public IActionResult Index()
        {
            return View();
        }

        // This action method returns the view that displays the privacy policy of the application.
        public IActionResult Privacy()
        {
            return View();
        }

        // This action method is responsible for handling errors.
        // It uses the [ResponseCache] attribute to prevent caching of the error page.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // The ErrorViewModel includes a RequestId that can be used to track down errors in the application logs.
            // It uses Activity.Current?.Id to get the current activity's ID if it exists, otherwise it uses the HttpContext's TraceIdentifier.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
