using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class DoctorIdInPrescriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DoctorId",
                table: "Prescriptions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Prescriptions");
        }
    }
}
