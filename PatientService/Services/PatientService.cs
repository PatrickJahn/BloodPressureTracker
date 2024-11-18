using AutoMapper;
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
            var patients = await patientRepository.GetAllPatientsAsync(); 
            return mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto> GetPatientByIdAsync(Guid patientId)
        {
            var patient = await patientRepository.GetPatientByIdAsync(patientId); 
            return patient == null ? null : mapper.Map<PatientDto>(patient);
        }

        public async Task AddPatientAsync(CreatePatientDto patientDto)
        {
            var patient = mapper.Map<Patient>(patientDto);
            await patientRepository.AddPatientAsync(patient); 
        }

        public async Task UpdatePatientAsync(Guid patientId, UpdatePatientDto patientDto)
        {
            var patient = await patientRepository.GetPatientByIdAsync(patientId); 
            if (patient != null)
            {
                mapper.Map(patientDto, patient); 
                await patientRepository.UpdatePatientAsync(patient); 
            }
        }

        public async Task DeletePatientAsync(Guid patientId)
        {
            await patientRepository.DeletePatientAsync(patientId); 
        }
    }
}