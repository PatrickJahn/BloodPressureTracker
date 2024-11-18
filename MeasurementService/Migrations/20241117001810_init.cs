using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeasurementService.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Systolic = table.Column<int>(nullable: false),
                    Diastolic = table.Column<int>(nullable: false),
                    Seen = table.Column<bool>(nullable: false),
                    PatientSSN = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurements");
        }

    }
}