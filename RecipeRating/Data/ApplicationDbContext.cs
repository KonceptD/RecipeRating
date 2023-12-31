﻿using Microsoft.AspNetCore.Identity;
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

        // public DbSet<AppUserModel> AppUsers { get; set; } // No need for this DbSet since AppUserModel is already included by the IdentityDbContext
        public DbSet<RecipeModel> Recipes { get; set; }
        public DbSet<RatingModel> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Custom model configurations as I was having validation errors with change of User Data and the 
            // program was wanting Data that wasn't even relevant in certain areas like Recipe data asking for UserID/Email etc

            modelBuilder.Entity<RecipeModel>()
                .HasOne(r => r.User) // One user
                .WithMany(u => u.Recipes) // to many recipes
                .HasForeignKey(r => r.UserID)
                .IsRequired(false); // User is optional, UserID can be null

            // Configure the one-to-many relationship between RecipeModel and RatingModel
            modelBuilder.Entity<RecipeModel>()
                .HasMany(r => r.Ratings) // One recipe
                .WithOne(rt => rt.Recipe) // to many ratings
                .HasForeignKey(rt => rt.RecipeID)
                .IsRequired(false); // Ratings are optional
        

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
