using System.ComponentModel.DataAnnotations;


namespace RecipeRating.Models.ViewModels
{
    public class CreateRecipeViewModel // Was getting validation errors when creating a recipe so I needed to make this to bypass that error
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
