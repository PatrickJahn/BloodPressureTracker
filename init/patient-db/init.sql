-- Create Database if not exists
IF DB_ID('PatientDB') IS NULL
BEGIN
    CREATE DATABASE PatientDB;
END;
GO

USE PatientDB;
GO

-- Create Patient Table
IF OBJECT_ID('dbo.Patient', 'U') IS NULL
BEGIN
CREATE TABLE Patient (
                         SSN NVARCHAR(50) PRIMARY KEY,
                         Mail NVARCHAR(100) NOT NULL,
                         Name NVARCHAR(100) NOT NULL
);

-- Insert Initial Data into Patient Table
INSERT INTO Patient (SSN, Mail, Name) VALUES
                                          ('123-45-6789', 'john.doe@example.com', 'John Doe'),
                                          ('987-65-4321', 'jane.smith@example.com', 'Jane Smith'),
                                          ('555-55-5555', 'alice.jones@example.com', 'Alice Jones');

END;
GO




