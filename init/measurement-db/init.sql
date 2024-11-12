-- Create Database if not exists
IF DB_ID('MeasurementDB') IS NULL
BEGIN
    CREATE DATABASE MeasurementDB;
END;
GO

USE MeasurementDB;
GO

-- Create Measurements Table
IF OBJECT_ID('dbo.Measurements', 'U') IS NULL
BEGIN
CREATE TABLE Measurements (
                              Id INT PRIMARY KEY IDENTITY(1,1),
                              Date DATETIME NOT NULL,
                              Systolic INT NOT NULL,
                              Diastolic INT NOT NULL,
                              Seen BIT NOT NULL
);


-- Insert Initial Data into Measurements Table
INSERT INTO Measurements (Date, Systolic, Diastolic, Seen) VALUES
                                                               (GETDATE(), 120, 80, 0),
                                                               (DATEADD(DAY, -1, GETDATE()), 125, 82, 1),
                                                               (DATEADD(DAY, -2, GETDATE()), 118, 78, 0),
                                                               (DATEADD(DAY, -3, GETDATE()), 130, 85, 1);


END;
GO
