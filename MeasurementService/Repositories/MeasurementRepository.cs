using Microsoft.EntityFrameworkCore;
using MeasurementService.Data;
using MeasurementService.Models;
using MeasurementService.Repositories.Interfaces;

namespace MeasurementService.Repositories
{
    public class MeasurementRepository(MeasurementDbContext context) : IMeasurementRepository
    {
        public async Task<IEnumerable<Measurement?>> GetAllMeasurementsAsync()
        {
            return await context.Measurements.ToListAsync();
        }

        public async Task<Measurement?> GetMeasurementByIdAsync(Guid MeasurementId)
        {
            return await context.Measurements.FindAsync(MeasurementId);
        }

        public async Task AddMeasurementAsync(Measurement? Measurement)
        {
            await context.Measurements.AddAsync(Measurement);
            await context.SaveChangesAsync();
        }

        public async Task DeleteMeasurementAsync(Guid MeasurementId)
        {
            var Measurement = await context.Measurements.FindAsync(MeasurementId);
            if (Measurement != null)
            {
                context.Measurements.Remove(Measurement);
                await context.SaveChangesAsync();
            }
        }
    }
}