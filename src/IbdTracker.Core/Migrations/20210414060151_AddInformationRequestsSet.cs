using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class AddInformationRequestsSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InformationRequest_Doctors_DoctorId",
                table: "InformationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_InformationRequest_Patients_PatientId",
                table: "InformationRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InformationRequest",
                table: "InformationRequest");

            migrationBuilder.RenameTable(
                name: "InformationRequest",
                newName: "InformationRequests");

            migrationBuilder.RenameIndex(
                name: "IX_InformationRequest_PatientId",
                table: "InformationRequests",
                newName: "IX_InformationRequests_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_InformationRequest_DoctorId",
                table: "InformationRequests",
                newName: "IX_InformationRequests_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InformationRequests",
                table: "InformationRequests",
                column: "InformationRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_InformationRequests_Doctors_DoctorId",
                table: "InformationRequests",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InformationRequests_Patients_PatientId",
                table: "InformationRequests",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InformationRequests_Doctors_DoctorId",
                table: "InformationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_InformationRequests_Patients_PatientId",
                table: "InformationRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InformationRequests",
                table: "InformationRequests");

            migrationBuilder.RenameTable(
                name: "InformationRequests",
                newName: "InformationRequest");

            migrationBuilder.RenameIndex(
                name: "IX_InformationRequests_PatientId",
                table: "InformationRequest",
                newName: "IX_InformationRequest_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_InformationRequests_DoctorId",
                table: "InformationRequest",
                newName: "IX_InformationRequest_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InformationRequest",
                table: "InformationRequest",
                column: "InformationRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_InformationRequest_Doctors_DoctorId",
                table: "InformationRequest",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InformationRequest_Patients_PatientId",
                table: "InformationRequest",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
