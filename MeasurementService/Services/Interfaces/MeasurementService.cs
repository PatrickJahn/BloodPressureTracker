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

        public async Task<IEnumerable<MeasurementDto>> GetMeasurementsBySSn(string ssn)
        {
            var measurements = await MeasurementRepository.GetAllMeasurementsBySSnAsync(ssn);
            return mapper.Map<IEnumerable<MeasurementDto>>(measurements);

        }

        public async Task<MeasurementDto> GetMeasurementByIdAsync(Guid MeasurementId)
        {
            var Measurement = await MeasurementRepository.GetMeasurementByIdAsync(MeasurementId);
            return mapper.Map<MeasurementDto>(Measurement);
        }

        public async Task AddMeasurementAsync(CreateMeasurementDto dto)
        {
            var measurement = new Measurement()
            {
                Date = DateTime.UtcNow,
                Seen = false,
                Diastolic = dto.Diastolic,
                Systolic = dto.Systolic,
                PatientSSN = dto.PatientSSn
            };
            await MeasurementRepository.AddMeasurementAsync(measurement);
        }

        public async Task DeleteMeasurementAsync(Guid MeasurementId)
        {
            await MeasurementRepository.DeleteMeasurementAsync(MeasurementId);
        }
    }
}