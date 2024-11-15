using PatientService.DTOs;

namespace PatientService.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
        Task<PatientDto> GetPatientByIdAsync(Guid patientId);
        Task AddPatientAsync(CreatePatientDto patientDto);
        Task UpdatePatientAsync(Guid patientId, UpdatePatientDto patientDto);
        Task DeletePatientAsync(Guid patientId);
    }
}