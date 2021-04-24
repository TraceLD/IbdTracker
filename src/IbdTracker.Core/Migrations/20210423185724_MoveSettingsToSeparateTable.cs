using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class MoveSettingsToSeparateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareData",
                table: "Patients");

            migrationBuilder.CreateTable(
                name: "PatientApplicationSettings",
                columns: table => new
                {
                    PatientId = table.Column<string>(type: "text", nullable: false),
                    ShareDataForResearch = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientApplicationSettings", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_PatientApplicationSettings_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientApplicationSettings");

            migrationBuilder.AddColumn<bool>(
                name: "ShareData",
                table: "Patients",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
