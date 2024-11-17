using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeasurementService.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Check if the Measurements table exists before creating it
            migrationBuilder.Sql(
                @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Measurements' AND xtype='U')
                BEGIN
                    CREATE TABLE Measurements (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Date DATETIME2 NOT NULL,
                        Systolic INT NOT NULL,
                        Diastolic INT NOT NULL,
                        Seen BIT NOT NULL,
                        PatientSSN NVARCHAR(MAX) NOT NULL
                    )
                END
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Check if the Measurements table exists before dropping it
            migrationBuilder.Sql(
                @"
                IF OBJECT_ID('dbo.Measurements', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE Measurements
                END
                ");
        }
    }
}