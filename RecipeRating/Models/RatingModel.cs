using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeRating.Models
{
    public class RatingModel
    {
        [Key]
        public int RatingID { get; set; } // Primary key for the Ratings table.

        [Required]
        public int Rating { get; set; } // The rating score given by a user to a recipe.

        [Required]
        public int RecipeID { get; set; } // Foreign key relating this rating to a specific recipe.

        [Required]
        public string UserID { get; set; } // Foreign key relating this rating to the user who made it.

        // Navigation properties to establish relationships in the Entity Framework
        public virtual AppUserModel User { get; set; } // Navigation property to the associated user.
        public virtual RecipeModel Recipe { get; set; } // Navigation property to the associated recipe.

        /* This is the SQL script from my inital DB to help me remember  
        /* 
         -- Create the Ratings table
            CREATE TABLE Ratings (
                RatingID INT PRIMARY KEY IDENTITY(1,1),
                Rating INT NOT NULL,
                RecipeID INT NOT NULL, -- Foreign key to Recipes table
                UserID INT NOT NULL,   -- Foreign key to Users table
                CONSTRAINT FK_Ratings_Recipes FOREIGN KEY (RecipeID) REFERENCES Recipes(RecipeID),
                CONSTRAINT FK_Ratings_Users FOREIGN KEY (UserID) REFERENCES Users(UserID)
            );
         */
    }
}
