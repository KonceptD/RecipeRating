

using System.ComponentModel.DataAnnotations;

namespace RecipeRating.Models
{
    public class AppUserModel
    {
        public int UserID { get; set; }

        [Key]
        [Required]
        [StringLength(255)]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [StringLength(255)]
        public required string Username { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(255)]
        public string? SecretQuestion { get; set; }

        [StringLength(255)]
        public string? SecretAnswer { get; set; }

        /*
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
