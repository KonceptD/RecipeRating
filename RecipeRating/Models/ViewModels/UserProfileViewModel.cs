using Microsoft.AspNetCore.Mvc;

namespace RecipeRating.Models.ViewModels
{
    public class UserProfileViewModel : Controller
    {
        public AppUserModel User { get; set; }
        public IEnumerable<RecipeModel> Recipes { get; set; }
        public IEnumerable<RatingModel> Ratings { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        
    }
}
