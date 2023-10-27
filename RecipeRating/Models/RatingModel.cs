using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeRating.Models
{
    public class RatingModel
    {
        [Key]
        public int RatingID { get; set; }

        [Required]
        public int Rating { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        // Navigation Properties
        public virtual AppUserModel User { get; set; }

        public virtual RecipeModel Recipe { get; set; }

        /* Database SQL definition, which you might use for manual creation */

        /* 
         -- Create the Ratings table
            CREATE TABLE Ratings (
                - RatingID INT PRIMARY KEY IDENTITY(1,1),
                - Rating INT NOT NULL,
                * RecipeID INT NOT NULL, -- Foreign key to Recipes table
                * UserID INT NOT NULL,   -- Foreign key to Users table
                CONSTRAINT FK_Ratings_Recipes FOREIGN KEY (RecipeID) REFERENCES Recipes(RecipeID),
                CONSTRAINT FK_Ratings_Users FOREIGN KEY (UserID) REFERENCES Users(UserID)
            );
         */
    }
}
