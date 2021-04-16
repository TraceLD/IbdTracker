using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class AddFoodItemDescriptionColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FoodItems",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "FoodItems");
        }
    }
}
