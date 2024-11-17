using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientService.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Check if the Patients table exists before creating it
            migrationBuilder.Sql(
                @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Patients' AND xtype='U')
                BEGIN
                    CREATE TABLE Patients (
                        PatientId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                        SSN NVARCHAR(MAX) NOT NULL,
                        Name NVARCHAR(100) NOT NULL,
                        Email NVARCHAR(MAX) NOT NULL,
                        DateOfBirth DATETIME2 NOT NULL,
                        Gender NVARCHAR(MAX) NOT NULL,
                        MedicalHistory NVARCHAR(MAX) NULL
                    )
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Check if the Patients table exists before dropping it
            migrationBuilder.Sql(
                @"
                IF OBJECT_ID('dbo.Patients', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE Patients
                END
                ");
        }
    }
}