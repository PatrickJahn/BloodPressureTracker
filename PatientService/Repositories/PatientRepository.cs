using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.Models;
using PatientService.Repositories.Interfaces;

namespace PatientService.Repositories
{
    public class PatientRepository(PatientDbContext context) : IPatientRepository
    {
        public async Task<IEnumerable<Patient?>> GetAllPatientsAsync()
        {
            return await context.Patients.ToListAsync();
        }

        public async Task<Patient?> GetPatientByIdAsync(Guid patientId)
        {
            return await context.Patients.FindAsync(patientId);
        }

        public async Task AddPatientAsync(Patient? patient)
        {
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePatientAsync(Patient? patient)
        {
            context.Patients.Update(patient);
            await context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(Guid patientId)
        {
            var patient = await context.Patients.FindAsync(patientId);
            if (patient != null)
            {
                context.Patients.Remove(patient);
                await context.SaveChangesAsync();
            }
        }
    }
}