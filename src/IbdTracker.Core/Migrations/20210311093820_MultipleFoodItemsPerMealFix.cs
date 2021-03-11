using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class MultipleFoodItemsPerMealFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Meals_MealId",
                table: "FoodItems");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_MealId",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "MealId",
                table: "FoodItems");

            migrationBuilder.CreateTable(
                name: "FoodItemMeal",
                columns: table => new
                {
                    FoodItemsFoodItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    MealsMealId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItemMeal", x => new { x.FoodItemsFoodItemId, x.MealsMealId });
                    table.ForeignKey(
                        name: "FK_FoodItemMeal_FoodItems_FoodItemsFoodItemId",
                        column: x => x.FoodItemsFoodItemId,
                        principalTable: "FoodItems",
                        principalColumn: "FoodItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodItemMeal_Meals_MealsMealId",
                        column: x => x.MealsMealId,
                        principalTable: "Meals",
                        principalColumn: "MealId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodItemMeal_MealsMealId",
                table: "FoodItemMeal",
                column: "MealsMealId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodItemMeal");

            migrationBuilder.AddColumn<Guid>(
                name: "MealId",
                table: "FoodItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_MealId",
                table: "FoodItems",
                column: "MealId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Meals_MealId",
                table: "FoodItems",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "MealId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
