using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class AddPrescriptionEndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                table: "Prescriptions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "Prescriptions");
        }
    }
}
