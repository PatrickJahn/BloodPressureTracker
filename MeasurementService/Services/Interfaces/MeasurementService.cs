using AutoMapper;
using MeasurementService.DTOs;
using MeasurementService.Models;
using MeasurementService.Repositories.Interfaces;
using MeasurementService.Services.Interfaces;

namespace MeasurementService.Services
{
    public class MeasurementService(IMeasurementRepository MeasurementRepository, IMapper mapper) : IMeasurementService
    {

        public async Task<IEnumerable<MeasurementDto>> GetAllMeasurementsAsync()
        {
            var Measurements = await MeasurementRepository.GetAllMeasurementsAsync();
            return mapper.Map<IEnumerable<MeasurementDto>>(Measurements);
        }

        public async Task<MeasurementDto> GetMeasurementByIdAsync(Guid MeasurementId)
        {
            var Measurement = await MeasurementRepository.GetMeasurementByIdAsync(MeasurementId);
            return mapper.Map<MeasurementDto>(Measurement);
        }

        public async Task AddMeasurementAsync(CreateMeasurementDto MeasurementDto)
        {
            var Measurement = mapper.Map<Measurement>(MeasurementDto);
            await MeasurementRepository.AddMeasurementAsync(Measurement);
        }

        public async Task DeleteMeasurementAsync(Guid MeasurementId)
        {
            await MeasurementRepository.DeleteMeasurementAsync(MeasurementId);
        }
    }
}