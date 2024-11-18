using AutoMapper;
using MeasurementService.DTOs;
using MeasurementService.Models;
using MeasurementService.Repositories.Interfaces;
using MeasurementService.Services.Interfaces;

namespace MeasurementService.Services
{
    public class MeasurementService(IMeasurementRepository measurementRepository, IMapper mapper)
        : IMeasurementService
    {
        public async Task<IEnumerable<MeasurementDto>> GetAllMeasurementsAsync()
        {
            var measurements = await measurementRepository.GetAllMeasurementsAsync();
            if (measurements == null) throw new ArgumentNullException(nameof(measurements));
            return mapper.Map<IEnumerable<MeasurementDto>>(measurements);
        }

        public async Task<IEnumerable<MeasurementDto>> GetMeasurementsBySSNAsync(string ssn)
        {
            var measurements = await measurementRepository.GetAllMeasurementsBySSNAsync(ssn);
            if (measurements == null) throw new ArgumentNullException(nameof(measurements));
            return mapper.Map<IEnumerable<MeasurementDto>>(measurements);
        }

        public async Task<MeasurementDto> GetMeasurementByIdAsync(Guid id)
        {
            var measurement = await measurementRepository.GetMeasurementByIdAsync(id);
            if (measurement == null) throw new KeyNotFoundException($"Measurement with ID {id} not found.");
            return mapper.Map<MeasurementDto>(measurement);
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
            await measurementRepository.AddMeasurementAsync(measurement);
        }

        public async Task DeleteMeasurementAsync(Guid id)
        {
            await measurementRepository.DeleteMeasurementAsync(id);
        }
    }
}
