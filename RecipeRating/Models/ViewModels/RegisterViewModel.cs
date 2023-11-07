using System.ComponentModel.DataAnnotations;

namespace RecipeRating.Models.ViewModels
{
    // RegisterViewModel is used to capture the necessary information from a user when registering on the site.
    public class RegisterViewModel
    {
        // Email is used as a unique identifier for login and communication.
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        // Username will be displayed across the site, such as on posts or recipes.
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        // Password for the user's account.
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // ConfirmPassword is used to validate that the entered passwords match.
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // Optional first name of the user.
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        // Optional last name of the user.
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        // A secret question for account recovery purposes.
        [DataType(DataType.Text)]
        public string SecretQuestion { get; set; }

        // The answer to the secret question.
        [DataType(DataType.Text)]
        public string SecretAnswer { get; set; }

        // Include properties for any other fields you need (e.g., FirstName, LastName).
    }
}
