using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class SeparateNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Appointments",
                newName: "PatientsNotes");

            migrationBuilder.AddColumn<string>(
                name: "DoctorsNotes",
                table: "Appointments",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorsNotes",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "PatientsNotes",
                table: "Appointments",
                newName: "Notes");
        }
    }
}
