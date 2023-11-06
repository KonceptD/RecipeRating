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

            // Check if there are any ratings before calculating the average
            var ratings = _context.Ratings.Where(r => r.RecipeID == id);
            double avgRating = 0; // Default value if there are no ratings

            if (ratings.Any()) // Check if there are any ratings
            {
                avgRating = await ratings.AverageAsync(r => r.Rating); // Calculate the average rating
            }

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

        // GET: Recipes/Edit/5
        [Authorize] // Ensure only authenticated users can access this
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

            // Check if the current user is the owner of the recipe
            if (recipe.UserID != _userManager.GetUserId(User))
            {
                return Forbid(); // or return View("Error") with a custom error message
            }

            return View(recipe);
        }

        // POST: Recipes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] // Ensure only authenticated users can post this
        public async Task<IActionResult> Edit(int id, [Bind("RecipeID,RecipeName,Ingredients,Method")] RecipeModel recipeModel)
        {
            if (id != recipeModel.RecipeID)
            {
                return NotFound();
            }

            // Remove the ModelState entries for the fields not present in the Edit form
            ModelState.Remove("UserID");
            ModelState.Remove("User");
            ModelState.Remove("Ratings");

            // The UserID should be set to the ID of the user who created the recipe.
            // Since UserID is not part of the form, fetch the current value and set it.
            var existingRecipe = await _context.Recipes.AsNoTracking().FirstOrDefaultAsync(r => r.RecipeID == id);
            if (existingRecipe == null)
            {
                return NotFound();
            }

            recipeModel.UserID = existingRecipe.UserID; // Preserve the UserID from the existing recipe

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
            // If we get to this point, something went wrong, so we re-display the form
            return View(recipeModel);
        }





        private bool RecipeModelExists(int id)
        {
            return _context.Recipes.Any(e => e.RecipeID == id);
        }


    }
}
