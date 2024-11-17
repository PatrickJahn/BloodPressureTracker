using MeasurementService.DTOs;

namespace MeasurementService.Services.Interfaces
{
    public interface IMeasurementService
    {
        Task<IEnumerable<MeasurementDto>> GetAllMeasurementsAsync();
        Task<IEnumerable<MeasurementDto>> GetMeasurementsBySSn(string ssn);

        Task<MeasurementDto> GetMeasurementByIdAsync(Guid MeasurementId);
        Task AddMeasurementAsync(CreateMeasurementDto MeasurementDto);
        Task DeleteMeasurementAsync(Guid MeasurementId);
    }
}