using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeRating.Models;

namespace RecipeRating.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        // No need for this DbSet since AppUserModel is already included by the IdentityDbContext
        // public DbSet<AppUserModel> AppUsers { get; set; }
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
                .OnDelete(DeleteBehavior.NoAction);  // No cascade delete

            modelBuilder.Entity<RatingModel>()
                .HasOne(r => r.Recipe)
                .WithMany(rc => rc.Ratings)
                .HasForeignKey(r => r.RecipeID)
                .OnDelete(DeleteBehavior.NoAction);  // No cascade delete

        }
    }
}
