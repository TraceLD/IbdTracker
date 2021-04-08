using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class FixTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "PatientsNotes",
                table: "Appointments",
                newName: "PatientNotes");

            migrationBuilder.RenameColumn(
                name: "DoctorsNotes",
                table: "Appointments",
                newName: "DoctorNotes");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId_StartDateTime",
                table: "Appointments",
                columns: new[] { "DoctorId", "StartDateTime" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId_StartDateTime",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "PatientNotes",
                table: "Appointments",
                newName: "PatientsNotes");

            migrationBuilder.RenameColumn(
                name: "DoctorNotes",
                table: "Appointments",
                newName: "DoctorsNotes");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");
        }
    }
}
