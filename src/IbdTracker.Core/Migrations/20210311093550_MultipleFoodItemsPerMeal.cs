using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class MultipleFoodItemsPerMeal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_FoodItems_FoodItemId",
                table: "Meals");

            migrationBuilder.DropIndex(
                name: "IX_Meals_FoodItemId",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "FoodItemId",
                table: "Meals");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Meals",
                type: "text",
                nullable: false,
                defaultValue: "");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Meals_MealId",
                table: "FoodItems");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_MealId",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "MealId",
                table: "FoodItems");

            migrationBuilder.AddColumn<Guid>(
                name: "FoodItemId",
                table: "Meals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Meals_FoodItemId",
                table: "Meals",
                column: "FoodItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_FoodItems_FoodItemId",
                table: "Meals",
                column: "FoodItemId",
                principalTable: "FoodItems",
                principalColumn: "FoodItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
