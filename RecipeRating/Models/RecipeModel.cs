using System.ComponentModel.DataAnnotations;

namespace RecipeRating.Models
{
    public class RecipeModel
    {
        [Key]
        public int RecipeID { get; set; }

        public int UserID { get; set; }

        [Required]
        public required string RecipeName { get; set; }

        [Required]
        public required string Ingredients { get; set; }

        [Required]
        public required string Method { get; set; }

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
