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

        public async Task<IEnumerable<Measurement?>> GetAllMeasurementsBySSNAsync(string ssn)
        {
            return await context.Measurements.Where(x => x.PatientSSN == ssn).ToListAsync();
        }

        public async Task<Measurement?> GetMeasurementByIdAsync(Guid id)
        {
            return await context.Measurements.FindAsync(id);
        }

        public async Task AddMeasurementAsync(Measurement? measurement)
        {
            await context.Measurements.AddAsync(measurement);
            await context.SaveChangesAsync();
        }

        public async Task DeleteMeasurementAsync(Guid id)
        {
            var measurement = await context.Measurements.FindAsync(id);
            if (measurement != null)
            {
                context.Measurements.Remove(measurement);
                await context.SaveChangesAsync();
            }
        }
    }
}