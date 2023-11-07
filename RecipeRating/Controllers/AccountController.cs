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
    // Controller for handling user account related requests such as register, login, logout, profile, etc.
    public class AccountController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager; // Manages user creation, deletion, etc.
        private readonly SignInManager<AppUserModel> _signInManager; // Manages user sign in and sign out operations
        private readonly ApplicationDbContext _context; // Database context for accessing the database

        // Constructor for the controller that receives instances of UserManager, SignInManager, and ApplicationDbContext through dependency injection
        public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // Returns the Register view
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // Handles the post request for registration
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) // Check if the model state is valid
            {
                var user = new AppUserModel // Create a new user instance
                {
                    UserName = model.Email, // Using Email for both UserName and Email
                    Email = model.Email,
                    UserDisplayName = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    SecretQuestion = model.SecretQuestion,
                    SecretAnswer = model.SecretAnswer
                };
                var result = await _userManager.CreateAsync(user, model.Password); // Create the user asynchronously
                if (result.Succeeded) // If the user creation was successful
                {
                    return RedirectToAction("Login", "Account"); // Redirect to login after registration
                }
                foreach (var error in result.Errors) // If there were errors, add them to the ModelState
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model); // Return the view with errors if the operation was not successful
        }

        // Returns the Login view
        public IActionResult Login()
        {
            return View();
        }

        // Handles the post request for login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid) // Check if the model state is valid
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded) // If the login was successful
                {
                    return RedirectToAction("Dashboard", "Account"); // Redirect to Dashboard after successful login
                }
                ModelState.AddModelError("", "Invalid login attempt"); // Add error to ModelState if login failed
            }
            return View(model); // Return the view with errors if login was not successful
        }

        // Handles the post request for logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Sign out the user
            return RedirectToAction("Index", "Home"); // Redirect to the home page
        }

        // Returns the Dashboard view for the logged-in user
        [Authorize] // This attribute ensures that only authenticated users can access the dashboard
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User); // Get the current logged-in user
            ViewBag.Username = user.UserName; // Passing the username to the view for display
            return View();
        }

        // Returns the Profile view for the logged-in user
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User); // Get the current logged-in user
            if (user == null)
            {
                return NotFound("User not found"); // Return a NotFound result if the user does not exist
            }

            // Include additional information, such as recipes and ratings by the user
            var recipes = await _context.Recipes.Where(r => r.UserID == user.Id).ToListAsync(); // Get the user's recipes
            var ratings = await _context.Ratings.Where(r => r.UserID == user.Id).ToListAsync(); // Get the user's ratings

            // Create the ViewModel with the user information, recipes, and ratings
            var viewModel = new UserProfileViewModel
            {
                User = user,
                Recipes = recipes,
                Ratings = ratings
            };

            return View(viewModel); // Return the view with the ViewModel
        }

        // Returns the EditProfile view for the logged-in user
        [Authorize]
        [HttpGet] // Explicitly indicating this action handles the HTTP GET method
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User); // Get the current logged-in user
            if (user == null)
            {
                return NotFound("User not found"); // Return a NotFound result if the user does not exist
            }

            if (!ModelState.IsValid) // Check if the model state is valid
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors); // Retrieve all errors from the ModelState
                // You can log these errors or inspect them in the debugger
            }

            // Reload the user from the database to ensure the information is current
            var userFromDb = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userFromDb == null)
            {
                return NotFound($"Unable to load user with ID '{user.Id}'."); // Return a NotFound result if the user does not exist in the database
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

            return View(viewModel); // Return the view with the ViewModel
        }

        // Handles the post request to update the user profile
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken] // Prevent cross-site request forgery
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid) // Check if the model state is valid
            {
                return View(model); // Return the view with errors
            }

            var user = await _userManager.GetUserAsync(User); // Get the current logged-in user
            if (user == null)
            {
                return NotFound("User not found"); // Return a NotFound result if the user does not exist
            }

            // Reload the user from the database to get the most up-to-date data
            var userFromDb = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userFromDb == null)
            {
                return NotFound($"Unable to load user with ID '{user.Id}'."); // Return a NotFound result if the user does not exist in the database
            }

            // Now, apply the changes from the model to the userFromDb object
            userFromDb.Email = model.Email;
            userFromDb.FirstName = model.FirstName;
            userFromDb.LastName = model.LastName;
            userFromDb.SecretQuestion = model.SecretQuestion;
            userFromDb.SecretAnswer = model.SecretAnswer;

            // Update the user in the database
            var result = await _userManager.UpdateAsync(userFromDb);
            if (result.Succeeded) // If the update was successful
            {
                return RedirectToAction(nameof(Profile)); // Redirect to the Profile view
            }
            else // If the update failed
            {
                foreach (var error in result.Errors) // Add the errors to the ModelState
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model); // Return the view with errors
            }
        }

        // Handles the post request to change the user password
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken] // Prevent cross-site request forgery
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) // Check if the model state is valid
            {
                return View(model); // Return the view with errors
            }

            var user = await _userManager.GetUserAsync(User); // Get the current logged-in user
            if (user == null)
            {
                return NotFound("User not found"); // Return a NotFound result if the user does not exist
            }

            // Attempt to change the user's password
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (changePasswordResult.Succeeded) // If the password change was successful
            {
                return RedirectToAction(nameof(Profile)); // Redirect to the Profile view
            }
            else // If the password change failed
            {
                foreach (var error in changePasswordResult.Errors) // Add the errors to the ModelState
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model); // Return the view with errors
            }
        }
    }
}
