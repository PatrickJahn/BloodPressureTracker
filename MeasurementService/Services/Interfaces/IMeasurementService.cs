using MeasurementService.DTOs;

namespace MeasurementService.Services.Interfaces
{
    public interface IMeasurementService
    {
        Task<IEnumerable<MeasurementDto>> GetAllMeasurementsAsync(); 
        Task<IEnumerable<MeasurementDto>> GetMeasurementsBySSNAsync(string ssn); 
        Task<MeasurementDto> GetMeasurementByIdAsync(Guid id); 
        Task AddMeasurementAsync(CreateMeasurementDto measurementDto); 
        Task DeleteMeasurementAsync(Guid id);
    }
}