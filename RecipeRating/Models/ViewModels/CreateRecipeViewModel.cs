using System.ComponentModel.DataAnnotations;


namespace RecipeRating.Models.ViewModels
{
    // CreateRecipeViewModel contains the fields required to create a new recipe.
    // It includes validation attributes to enforce input constraints.
    public class CreateRecipeViewModel // Was getting validation errors when creating a recipe so I needed to make this to bypass that error
    {
        // Name of the recipe, with a maximum length of 255 characters.
        [Required]
        [StringLength(255)]
        public string RecipeName { get; set; }

        // Ingredients used in the recipe.
        [Required]
        public string Ingredients { get; set; }

        // The method or steps to create the recipe.
        [Required]
        public string Method { get; set; }

        // Add any additional fields required for creating a recipe.
    }

}
