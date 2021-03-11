using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class AddMealEventsFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MealEvents_PatientId",
                table: "MealEvents",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealEvents_Patients_PatientId",
                table: "MealEvents",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealEvents_Patients_PatientId",
                table: "MealEvents");

            migrationBuilder.DropIndex(
                name: "IX_MealEvents_PatientId",
                table: "MealEvents");
        }
    }
}
