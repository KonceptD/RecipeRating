using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeRating.Models;

namespace RecipeRating.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<AppUserModel> AppUsers { get; set; }
        public DbSet<RecipeModel> Recipes { get; set; }
        public DbSet<RatingModel> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ... Your custom model configurations ...
            modelBuilder.Entity<RatingModel>()
            .HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserID)
            .OnDelete(DeleteBehavior.Cascade);  // Keep this cascade

            modelBuilder.Entity<RatingModel>()
                .HasOne(r => r.Recipe)
                .WithMany(rcp => rcp.Ratings)
                .HasForeignKey(r => r.RecipeID)
                .OnDelete(DeleteBehavior.Restrict);  // Change this to restrict
        }
    }
}
