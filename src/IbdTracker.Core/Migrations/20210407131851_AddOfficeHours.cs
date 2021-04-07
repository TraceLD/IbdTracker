using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class AddOfficeHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OfficeHours",
                table: "Doctors",
                type: "jsonb",
                nullable: false,
                defaultValue: "[]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfficeHours",
                table: "Doctors");
        }
    }
}
