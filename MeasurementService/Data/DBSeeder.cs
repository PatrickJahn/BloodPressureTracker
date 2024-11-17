using MeasurementService.Models;

namespace MeasurementService.Data;

public class DBSeeder
{
    public static void Seed(MeasurementDbContext dbContext)
    {
        if (!dbContext.Measurements.Any())
        {
            dbContext.Measurements.AddRange(
                new Measurement { Date = DateTime.UtcNow, Seen = false, Diastolic = 201, Systolic = 120, PatientSSN = "123-45-678"},
                new Measurement { Date = DateTime.UtcNow, Seen = true, Diastolic = 129, Systolic = 230, PatientSSN = "987-65-4321"},
                new Measurement { Date = DateTime.UtcNow.AddDays(2), Seen = true, Diastolic = 142, Systolic = 240, PatientSSN = "987-65-4321"}

            );
            dbContext.SaveChanges();
        }
    }
}