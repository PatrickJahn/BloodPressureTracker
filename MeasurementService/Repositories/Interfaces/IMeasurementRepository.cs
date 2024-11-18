using MeasurementService.Models;

namespace MeasurementService.Repositories.Interfaces
{
    public interface IMeasurementRepository
    {
        Task<IEnumerable<Measurement?>> GetAllMeasurementsAsync();
        Task<IEnumerable<Measurement?>> GetAllMeasurementsBySSNAsync(string ssn);
        Task<Measurement?> GetMeasurementByIdAsync(Guid id); 
        Task AddMeasurementAsync(Measurement? measurement); 
        Task DeleteMeasurementAsync(Guid id); 
    }
}