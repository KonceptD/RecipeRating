using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeRating.Migrations
{
    /// <inheritdoc />
    public partial class AdjustCascadeBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AppUserModel_UserID",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Recipes_RecipeID",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AppUserModel_UserID",
                table: "Recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserModel",
                table: "AppUserModel");

            migrationBuilder.RenameTable(
                name: "AppUserModel",
                newName: "AppUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AppUsers_UserID",
                table: "Ratings",
                column: "UserID",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Recipes_RecipeID",
                table: "Ratings",
                column: "RecipeID",
                principalTable: "Recipes",
                principalColumn: "RecipeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AppUsers_UserID",
                table: "Recipes",
                column: "UserID",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AppUsers_UserID",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Recipes_RecipeID",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AppUsers_UserID",
                table: "Recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers");

            migrationBuilder.RenameTable(
                name: "AppUsers",
                newName: "AppUserModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserModel",
                table: "AppUserModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AppUserModel_UserID",
                table: "Ratings",
                column: "UserID",
                principalTable: "AppUserModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Recipes_RecipeID",
                table: "Ratings",
                column: "RecipeID",
                principalTable: "Recipes",
                principalColumn: "RecipeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AppUserModel_UserID",
                table: "Recipes",
                column: "UserID",
                principalTable: "AppUserModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
