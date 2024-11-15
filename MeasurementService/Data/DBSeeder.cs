using MeasurementService.Models;

namespace MeasurementService.Data;

public class DBSeeder
{
    public static void Seed(MeasurementDbContext dbContext)
    {
        if (!dbContext.Measurements.Any())
        {
            dbContext.Measurements.AddRange(
                new Measurement { Date = DateTime.UtcNow, Seen = false, Diastolic = 201, Systolic = 120},
                new Measurement { Date = DateTime.UtcNow, Seen = true, Diastolic = 129, Systolic = 230}
            );
            dbContext.SaveChanges();
        }
    }
}