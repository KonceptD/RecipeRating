using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeRating.Data;
using RecipeRating.Models;
using RecipeRating.Models.ViewModels;
using System.Security.Claims;

namespace RecipeRating.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUserModel> _userManager;

        public RecipesController(ApplicationDbContext context, UserManager<AppUserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            return View(await _context.Recipes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FirstOrDefaultAsync(m => m.RecipeID == id);
            if (recipe == null)
            {
                return NotFound();
            }

            // Calculate the average rating
            var avgRating = _context.Ratings
                .Where(r => r.RecipeID == id)
                .DefaultIfEmpty()  // This ensures that if there are no ratings, we won't get an exception
                .Average(r => r.Rating);

            // Add the average rating to the ViewData or ViewModel
            ViewData["AverageRating"] = avgRating;

            return View(recipe);
        }

        public IActionResult Create()
        {
            return View(new CreateRecipeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecipeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Map the ViewModel to your RecipeModel
                    var recipe = new RecipeModel
                    {
                        UserID = user.Id, // This will be set from the logged-in user
                        RecipeName = model.RecipeName,
                        Ingredients = model.Ingredients,
                        Method = model.Method
                        // Map other fields if necessary
                    };

                    _context.Add(recipe);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Dashboard", "Account");
                }
                else
                {
                    // Handle the case where the user is not found (e.g., return an error message)
                }
            }
            return View(model); // If ModelState is invalid, pass the model back to the view
        }


        [Authorize] // view the recipes of the currently logged-in user
        public async Task<IActionResult> UserRecipes()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var recipes = await _context.Recipes.Where(r => r.UserID == user.Id).ToListAsync();
            return View(recipes);
        }


        [HttpPost]
        [Authorize] // Ensures only logged in users can rate the recipes
        public async Task<IActionResult> RateRecipe(int ratingValue, int recipeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the user's ID

            var existingRating = _context.Ratings
                .FirstOrDefault(r => r.RecipeID == recipeId && r.UserID == userId);

            if (existingRating != null)
            {
                existingRating.Rating = ratingValue;
            }
            else
            {
                var rating = new RatingModel
                {
                    Rating = ratingValue,
                    RecipeID = recipeId,
                    UserID = userId
                };
                _context.Ratings.Add(rating);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = recipeId });
        }

    }
}
