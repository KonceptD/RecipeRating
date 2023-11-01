using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeRating.Models;
using RecipeRating.Models.ViewModels;

namespace RecipeRating.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly SignInManager<AppUserModel> _signInManager;

        public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUserModel
                {
                    UserName = model.Email,  // Using Email for both UserName and Email
                    Email = model.Email,
                    UserDisplayName = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    SecretQuestion = model.SecretQuestion,
                    SecretAnswer = model.SecretAnswer
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Remove the line that signs the user in
                    // await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login", "Account"); // Redirect to login after registration
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Dashboard", "Account"); // Redirect to Dashboard after successful login
                }
                ModelState.AddModelError("", "Invalid login attempt");
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // For the User's Dashboard
        [Authorize]  // This attribute ensures that only authenticated users can access the dashboard
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Username = user.UserName; // Passing the username to the view for display
            return View();
        }



    }

}
