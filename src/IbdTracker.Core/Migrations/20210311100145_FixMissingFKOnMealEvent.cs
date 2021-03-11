using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class FixMissingFKOnMealEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MealEvents_MealId",
                table: "MealEvents",
                column: "MealId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealEvents_Meals_MealId",
                table: "MealEvents",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "MealId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealEvents_Meals_MealId",
                table: "MealEvents");

            migrationBuilder.DropIndex(
                name: "IX_MealEvents_MealId",
                table: "MealEvents");
        }
    }
}
