using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeRating.Models;

namespace RecipeRating.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUserModel, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<RecipeModel> Recipes { get; set; }
        public DbSet<RatingModel> Ratings { get; set; }
    }
}
