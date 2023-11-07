using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RecipeRating.Models.ViewModels
{
    // EditProfileViewModel is used to handle the data when a user wants to edit their profile.
    public class EditProfileViewModel
    {
        // User's email, which may or may not be editable depending on your application's requirements.
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // User's first name, optional for the user to provide.
        [StringLength(50)]
        public string? FirstName { get; set; }

        // User's last name, optional for the user to provide.
        [StringLength(50)]
        public string? LastName { get; set; }

        // A secret question that the user can set for account recovery.
        [StringLength(255)]
        public string? SecretQuestion { get; set; }

        // The answer to the secret question for account recovery.
        [StringLength(255)]
        public string? SecretAnswer { get; set; }



    }
}
