using PatientService.Models;

namespace PatientService.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient?>> GetAllPatientsAsync();
        Task<Patient?> GetPatientByIdAsync(Guid patientId);
        Task AddPatientAsync(Patient? patient);
        Task UpdatePatientAsync(Patient? patient);
        Task DeletePatientAsync(Guid patientId);
    }
}