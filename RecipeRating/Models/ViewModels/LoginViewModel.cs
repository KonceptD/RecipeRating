using System.ComponentModel.DataAnnotations;

namespace RecipeRating.Models.ViewModels
{
    // LoginViewModel represents the data needed for a user to log in.
    public class LoginViewModel
    {
        // Email associated with the user's account.
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        // Password for the user's account.
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // RememberMe indicates whether the user's session should be persistent.
        public bool RememberMe { get; set; }
    }
}
