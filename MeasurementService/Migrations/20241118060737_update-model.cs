using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeasurementService.Migrations
{
    /// <inheritdoc />
    public partial class updatemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegionCode",
                table: "Measurements");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegionCode",
                table: "Measurements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
