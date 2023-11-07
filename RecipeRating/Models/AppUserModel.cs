using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RecipeRating.Models
{
    public class AppUserModel : IdentityUser
    {
        [StringLength(50)]
        public string UserDisplayName { get; set; } // Display name for the user, separate from the username.

        [StringLength(50)]
        public string? FirstName { get; set; } // Optional first name of the user.

        [StringLength(50)]
        public string? LastName { get; set; } // Optional last name of the user.

        [StringLength(255)]
        public string? SecretQuestion { get; set; } // Optional secret question for account recovery.

        [StringLength(255)]
        public string? SecretAnswer { get; set; } // Optional answer to the secret question.

        // Collections to establish one-to-many relationships with ratings and recipes
        public ICollection<RatingModel> Ratings { get; set; } // Collection of ratings given by the user.
        public virtual ICollection<RecipeModel> Recipes { get; set; } // Collection of recipes created by the user.



        // The commented-out properties below are not necessary as they are already 
        // included in the base IdentityUser class that AppUserModel is inheriting from.

        /* public int UserID { get; set; } -- Not needed as IdentityUser already has an ID prop 

        [Key]
        [Required]
        [StringLength(255)]
        [EmailAddress]
        public required string Email { get; set; } -- Not needed as IdentityUser already has an Email prop

        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public required string Password { get; set; } -- Not needed as IdentityUser already has an Password prop

        [Required]
        [StringLength(255)]
        public required string Username { get; set; } -- Not needed as IdentityUser already has an Username prop
        */



        /* This is the SQL script from my inital DB to help me remember  
         -- Create the Users table
            CREATE TABLE Users (
                * UserID INT PRIMARY KEY IDENTITY(1,1),
                - Email NVARCHAR(255) NOT NULL,
                - Password NVARCHAR(255) NOT NULL,
                - Username NVARCHAR(255) NOT NULL,
                - FirstName NVARCHAR(50) NULL, -- Nullable
                - LastName NVARCHAR(50) NULL,  -- Nullable
                - SecretQuestion NVARCHAR(255) NULL, -- Nullable
                - SecretAnswer NVARCHAR(255) NULL   -- Nullable
            );
         */
    }
}
