using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RecipeRating.Models.ViewModels
{
    // ChangePasswordViewModel handles data for changing a user's password.
    public class ChangePasswordViewModel
    {
        // OldPassword is the current password for the user's account.
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }

        // NewPassword is the new password that the user wants to set.
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        // ConfirmPassword is used to confirm the new password entered by the user.
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


    }
}
