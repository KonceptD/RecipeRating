using System.ComponentModel.DataAnnotations;

namespace RecipeRating.Models.ViewModels
{
    public class CreateRecipeViewModel
    {
        [Required]
        [StringLength(255)]
        public string RecipeName { get; set; }

        [Required]
        public string Ingredients { get; set; }

        [Required]
        public string Method { get; set; }

        // You can add any other fields required for creating a recipe.
    }

}
