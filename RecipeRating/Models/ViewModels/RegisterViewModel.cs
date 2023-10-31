using System.ComponentModel.DataAnnotations;

namespace RecipeRating.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; } // is used for login and correspondence.

        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; } // stored as UserDisplayName in the AppUserModel, is used for display on the site like posts/recipes.

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Text)]
        public string FirstName { get; set; }

      
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        public string SecretQuestion { get; set; }

        [DataType(DataType.Text)]
        public string SecretAnswer { get; set; }

        // Include properties for any other fields you need (e.g., FirstName, LastName).
    }
}
