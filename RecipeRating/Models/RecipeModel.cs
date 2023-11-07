using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeRating.Models
{
    public class RecipeModel
    {
        [Key]
        public int RecipeID { get; set; } // Primary key for the Recipe table.

        [Required]
        [StringLength(255)]
        public string RecipeName { get; set; } // Name of the recipe, with a maximum length validation.

        [Required]
        public string Ingredients { get; set; } // Ingredients used in the recipe, required with no length limit.

        [Required]
        public string Method { get; set; } // Cooking instructions or method of preparation, required.

        [ForeignKey("User")]
        public string UserID { get; set; } // Foreign Key relation to the User who posted the recipe.

        // Navigation properties for Entity Framework relationships
        public virtual AppUserModel User { get; set; } // Navigation property to the associated user.
        public virtual List<RatingModel> Ratings { get; set; } // Collection of ratings for this recipe.

        /* This is the SQL script from my inital DB to help me remember 
         * 
         -- Create the Recipes table
        CREATE TABLE Recipes (
            - RecipeID INT PRIMARY KEY IDENTITY(1,1),
            * RecipeName NVARCHAR(255) NOT NULL,
            * Ingredients NVARCHAR(MAX) NOT NULL,
            * Method NVARCHAR(MAX) NOT NULL,
            - UserID INT NOT NULL, -- Foreign key to Users table
            CONSTRAINT FK_Recipes_Users FOREIGN KEY (UserID) REFERENCES Users(UserID)
            );
         */
    }
}
