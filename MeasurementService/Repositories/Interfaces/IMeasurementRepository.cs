using MeasurementService.Models;

namespace MeasurementService.Repositories.Interfaces
{
    public interface IMeasurementRepository
    {
        Task<IEnumerable<Measurement?>> GetAllMeasurementsAsync();
        Task<IEnumerable<Measurement?>> GetAllMeasurementsBySSnAsync(string ssn);

        Task<Measurement?> GetMeasurementByIdAsync(Guid MeasurementId);
        Task AddMeasurementAsync(Measurement? Measurement);
        Task DeleteMeasurementAsync(Guid MeasurementId);
    }
}