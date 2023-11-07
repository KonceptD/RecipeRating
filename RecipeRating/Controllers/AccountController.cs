using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeRating.Data;
using RecipeRating.Models;
using RecipeRating.Models.ViewModels;

namespace RecipeRating.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly SignInManager<AppUserModel> _signInManager;
        private readonly ApplicationDbContext _context; // Add this line


        public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context; // Set the context here

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

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // You can include additional information, such as recipes and ratings by the user
            var recipes = await _context.Recipes.Where(r => r.UserID == user.Id).ToListAsync();
            var ratings = await _context.Ratings.Where(r => r.UserID == user.Id).ToListAsync();

            var viewModel = new UserProfileViewModel
            {
                User = user,
                Recipes = recipes,
                Ratings = ratings
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpGet] // Explicitly indicating this action handles the HTTP GET method
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                // You can log these errors or inspect them in the debugger
            }


            // Reload the user from the database to ensure the information is current
            var userFromDb = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userFromDb == null)
            {
                return NotFound($"Unable to load user with ID '{user.Id}'.");
            }

            // Create the ViewModel using the most up-to-date data
            var viewModel = new EditProfileViewModel
            {
                Email = userFromDb.Email,
                FirstName = userFromDb.FirstName,
                LastName = userFromDb.LastName,
                SecretQuestion = userFromDb.SecretQuestion,
                SecretAnswer = userFromDb.SecretAnswer
                // Populate other fields as necessary
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Reload the user from the database to get the most up-to-date data
            var userFromDb = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userFromDb == null)
            {
                return NotFound($"Unable to load user with ID '{user.Id}'.");
            }

            // Now, apply the changes from the model to the userFromDb object
            userFromDb.Email = model.Email;
            userFromDb.FirstName = model.FirstName;
            userFromDb.LastName = model.LastName;
            userFromDb.SecretQuestion = model.SecretQuestion;
            userFromDb.SecretAnswer = model.SecretAnswer;


            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Handle success, maybe set a success message and redirect
                return RedirectToAction(nameof(Profile));
            }
            else
            {
                // Handle the case where update failed
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (changePasswordResult.Succeeded)
            {
                // Handle success
                return RedirectToAction(nameof(Profile));
            }
            else
            {
                // Handle failure
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }


    }

}
