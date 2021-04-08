using System.Collections.Generic;
using IbdTracker.Core;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IbdTracker.Core.Migrations
{
    public partial class NoManualConversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<OfficeHours>>(
                name: "OfficeHours",
                table: "Doctors",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldDefaultValue: "[]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OfficeHours",
                table: "Doctors",
                type: "jsonb",
                nullable: false,
                defaultValue: "[]",
                oldClrType: typeof(List<OfficeHours>),
                oldType: "jsonb");
        }
    }
}
