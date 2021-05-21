using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class Index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_StartDateTime",
                table: "Prescriptions",
                column: "StartDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_BnfChemicalSubstance",
                table: "Medications",
                column: "BnfChemicalSubstance");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_BnfProduct",
                table: "Medications",
                column: "BnfProduct");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_Name",
                table: "FoodItems",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_StartDateTime",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Medications_BnfChemicalSubstance",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Medications_BnfProduct",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_Name",
                table: "FoodItems");
        }
    }
}
