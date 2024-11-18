using Microsoft.EntityFrameworkCore;
using MeasurementService.Data;
using MeasurementService.Models;
using MeasurementService.Repositories.Interfaces;
using Monitoring;

namespace MeasurementService.Repositories
{
    public class MeasurementRepository(MeasurementDbContext context) : IMeasurementRepository
    {
        public async Task<IEnumerable<Measurement?>> GetAllMeasurementsAsync()
        {
            using var activity = LoggingService.activitySource.StartActivity();

            return await context.Measurements.ToListAsync();
        }

        public async Task<IEnumerable<Measurement?>> GetAllMeasurementsBySSNAsync(string ssn)
        {
            using var activity = LoggingService.activitySource.StartActivity();
            return await context.Measurements.Where(x => x.PatientSSN == ssn).ToListAsync();
        }

        public async Task<Measurement?> GetMeasurementByIdAsync(Guid id)
        {
            using var activity = LoggingService.activitySource.StartActivity();
            return await context.Measurements.FindAsync(id);

        }

        public async Task AddMeasurementAsync(Measurement? measurement)
        {
            using var activity = LoggingService.activitySource.StartActivity();
            await context.Measurements.AddAsync(measurement);

            await context.SaveChangesAsync();
        }

        public async Task DeleteMeasurementAsync(Guid id)
        {
            using var activity = LoggingService.activitySource.StartActivity();
            var measurement = await context.Measurements.FindAsync(id);
            if (measurement != null)
            {
                context.Measurements.Remove(measurement);
                await context.SaveChangesAsync();
            }
        }
    }
}