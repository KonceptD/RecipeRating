using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeRating.Data;
using RecipeRating.Models;
using RecipeRating.Models.ViewModels;
using System.Security.Claims;

namespace RecipeRating.Controllers

    /*The RecipesController provides functionality for:

    Displaying all recipes and details of a single recipe
    Creating new recipes
    Editing existing recipes
    Rating recipes
    Viewing recipes created by the logged-in user

    Each method is secured with appropriate authorization checks to ensure only authorized users can perform certain actions, such as editing or rating recipes. 
    The controller interacts with the ApplicationDbContext to perform CRUD operations and uses the UserManager to access user information based on the logged-in user. 
    The controller's methods also handle various states of a request, such as handling null IDs, not found entities, and verifying ownership of resources.
     */
{
    // The RecipesController is responsible for handling actions related to recipe management.
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUserModel> _userManager;

        // Constructor initializes the database context and user manager to interact with user and recipe data.
        public RecipesController(ApplicationDbContext context, UserManager<AppUserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Asynchronously gets and displays a list of recipes from the database.
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _context.Recipes.ToListAsync());
        }

        // Asynchronously displays the details of a specific recipe by ID.
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

            // If ratings exist for the recipe, calculate the average rating.
            var ratings = _context.Ratings.Where(r => r.RecipeID == id);
            double avgRating = 0;

            if (ratings.Any())
            {
                avgRating = await ratings.AverageAsync(r => r.Rating);
            }

            // Pass the average rating to the view using ViewData.
            ViewData["AverageRating"] = avgRating;

            return View(recipe);
        }

        // Returns a view for creating a new recipe.
        public IActionResult Create()
        {
            return View(new CreateRecipeViewModel());
        }

        // Handles the post request for creating a new recipe.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecipeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Create a new RecipeModel instance from the view model.
                    var recipe = new RecipeModel
                    {
                        UserID = user.Id,
                        RecipeName = model.RecipeName,
                        Ingredients = model.Ingredients,
                        Method = model.Method
                    };

                    _context.Add(recipe);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Dashboard", "Account");
                }
                else
                {
                    // Handle the case where the user is not found.
                }
            }
            // Return to the Create view with the model if ModelState is invalid.
            return View(model);
        }

        // Retrieves recipes created by the currently logged-in user.
        [Authorize]
        public async Task<IActionResult> UserRecipes()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var recipes = await _context.Recipes.Where(r => r.UserID == user.Id).ToListAsync();
            return View(recipes);
        }

        // Handles the post request to rate a recipe.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RateRecipe(int ratingValue, int recipeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingRating = _context.Ratings.FirstOrDefault(r => r.RecipeID == recipeId && r.UserID == userId);

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

        // Returns a view for editing an existing recipe.
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            // Verify that the current user is the owner of the recipe.
            if (recipe.UserID != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            return View(recipe);
        }

        // Handles the post request for editing an existing recipe.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeID,RecipeName,Ingredients,Method")] RecipeModel recipeModel)
        {
            if (id != recipeModel.RecipeID)
            {
                return NotFound();
            }

            // Remove ModelState entries for fields that aren't present in the form.
            ModelState.Remove("UserID");
            ModelState.Remove("User");
            ModelState.Remove("Ratings");

            var existingRecipe = await _context.Recipes.AsNoTracking().FirstOrDefaultAsync(r => r.RecipeID == id);
            if (existingRecipe == null)
            {
                return NotFound();
            }

            recipeModel.UserID = existingRecipe.UserID; // Keep the original UserID.

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = recipeModel.RecipeID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeModelExists(recipeModel.RecipeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            // Re-display the form if something went wrong.
            return View(recipeModel);
        }

        // Checks if a recipe with a given ID exists.
        private bool RecipeModelExists(int id)
        {
            return _context.Recipes.Any(e => e.RecipeID == id);
        }
    }
}
