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

        public async Task<IEnumerable<Measurement?>> GetAllMeasurementsBySSnAsync(string ssn)
        {
            using var activity = LoggingService.activitySource.StartActivity();

            return await context.Measurements.Where(x => x.PatientSSN.Equals(ssn)).ToListAsync();
        }

        public async Task<Measurement?> GetMeasurementByIdAsync(Guid MeasurementId)
        {
            using var activity = LoggingService.activitySource.StartActivity();

            return await context.Measurements.FindAsync(MeasurementId);
        }

        public async Task AddMeasurementAsync(Measurement? Measurement)
        {
            using var activity = LoggingService.activitySource.StartActivity();

            await context.Measurements.AddAsync(Measurement);
            await context.SaveChangesAsync();
        }

        public async Task DeleteMeasurementAsync(Guid MeasurementId)
        {
            using var activity = LoggingService.activitySource.StartActivity();

            var Measurement = await context.Measurements.FindAsync(MeasurementId);
            if (Measurement != null)
            {
                context.Measurements.Remove(Measurement);
                await context.SaveChangesAsync();
            }
        }
    }
}