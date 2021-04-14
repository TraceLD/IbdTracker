using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class AddInformationRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InformationRequest",
                columns: table => new
                {
                    InformationRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<string>(type: "text", nullable: false),
                    DoctorId = table.Column<string>(type: "text", nullable: false),
                    RequestedDataFrom = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RequestDataTo = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RequestedPain = table.Column<bool>(type: "boolean", nullable: false),
                    RequestedBms = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationRequest", x => x.InformationRequestId);
                    table.ForeignKey(
                        name: "FK_InformationRequest_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InformationRequest_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InformationRequest_DoctorId",
                table: "InformationRequest",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_InformationRequest_PatientId",
                table: "InformationRequest",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InformationRequest");
        }
    }
}
