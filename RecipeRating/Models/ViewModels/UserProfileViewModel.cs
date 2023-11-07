using Microsoft.AspNetCore.Mvc;

namespace RecipeRating.Models.ViewModels
{
    public class UserProfileViewModel
    {
        // UserProfileViewModel is designed to pass data about a user's profile to the view.
        // It contains the user's details, the recipes they've submitted, and the ratings they've given.
        public AppUserModel User { get; set; }

        // Recipes collection represents the recipes submitted by the user.
        public IEnumerable<RecipeModel> Recipes { get; set; }

        // Ratings collection represents the ratings provided by the user.
        public IEnumerable<RatingModel> Ratings { get; set; }


        
    }
}
