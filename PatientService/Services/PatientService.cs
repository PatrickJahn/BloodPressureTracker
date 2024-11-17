using AutoMapper;
using Monitoring;
using PatientService.DTOs;
using PatientService.Models;
using PatientService.Repositories.Interfaces;
using PatientService.Services.Interfaces;

namespace PatientService.Services
{
    public class PatientService(IPatientRepository patientRepository, IMapper mapper) : IPatientService
    {

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            using var activity = LoggingService.activitySource.StartActivity();

            var patients = await patientRepository.GetAllPatientsAsync();
            return mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto> GetPatientByIdAsync(Guid patientId)
        {
            using var activity = LoggingService.activitySource.StartActivity();

            var patient = await patientRepository.GetPatientByIdAsync(patientId);
            return mapper.Map<PatientDto>(patient);
        }

        public async Task AddPatientAsync(CreatePatientDto patientDto)
        {
            using var activity = LoggingService.activitySource.StartActivity();

            var patient = mapper.Map<Patient>(patientDto);
            await patientRepository.AddPatientAsync(patient);
        }

        public async Task UpdatePatientAsync(Guid patientId, UpdatePatientDto patientDto)
        {
            using var activity = LoggingService.activitySource.StartActivity();

            var patient = await patientRepository.GetPatientByIdAsync(patientId);
            if (patient != null)
            {
                mapper.Map(patientDto, patient);
                await patientRepository.UpdatePatientAsync(patient);
            }
        }

        public async Task DeletePatientAsync(Guid patientId)
        {
            using var activity = LoggingService.activitySource.StartActivity();

            await patientRepository.DeletePatientAsync(patientId);
        }
    }
}