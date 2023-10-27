using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeRating.Models
{
    public class RecipeModel
    {
        [Key]
        public int RecipeID { get; set; }

        [Required]
        [StringLength(255)]
        public string RecipeName { get; set; }

        [Required]
        public string Ingredients { get; set; }

        [Required]
        public string Method { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; } // Foreign Key

        // Navigation properties
        public virtual AppUserModel User { get; set; }
        public virtual List<RatingModel> Ratings { get; set; }

        /* 
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
