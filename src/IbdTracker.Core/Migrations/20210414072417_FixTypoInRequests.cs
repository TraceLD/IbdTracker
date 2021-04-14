using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class FixTypoInRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestDataTo",
                table: "InformationRequests",
                newName: "RequestedDataTo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestedDataTo",
                table: "InformationRequests",
                newName: "RequestDataTo");
        }
    }
}
