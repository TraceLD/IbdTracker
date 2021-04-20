using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class AddBnfDrugInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "Medications");

            migrationBuilder.RenameColumn(
                name: "Dosage",
                table: "Prescriptions",
                newName: "DoctorInstructions");

            migrationBuilder.RenameColumn(
                name: "ActiveIngredient",
                table: "Medications",
                newName: "BnfSubparagraph");

            migrationBuilder.AddColumn<string>(
                name: "BnfChapter",
                table: "Medications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BnfChapterCode",
                table: "Medications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BnfChemicalSubstance",
                table: "Medications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BnfChemicalSubstanceCode",
                table: "Medications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BnfParagraph",
                table: "Medications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BnfParagraphCode",
                table: "Medications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BnfPresentation",
                table: "Medications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BnfPresentationCode",
                table: "Medications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BnfProduct",
                table: "Medications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BnfProductCode",
                table: "Medications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BnfSection",
                table: "Medications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BnfSectionCode",
                table: "Medications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BnfSubparagraphCode",
                table: "Medications",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BnfChapter",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "BnfChapterCode",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "BnfChemicalSubstance",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "BnfChemicalSubstanceCode",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "BnfParagraph",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "BnfParagraphCode",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "BnfPresentation",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "BnfPresentationCode",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "BnfProduct",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "BnfProductCode",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "BnfSection",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "BnfSectionCode",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "BnfSubparagraphCode",
                table: "Medications");

            migrationBuilder.RenameColumn(
                name: "DoctorInstructions",
                table: "Prescriptions",
                newName: "Dosage");

            migrationBuilder.RenameColumn(
                name: "BnfSubparagraph",
                table: "Medications",
                newName: "ActiveIngredient");

            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "Medications",
                type: "text",
                nullable: true);
        }
    }
}
