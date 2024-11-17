using Microsoft.EntityFrameworkCore;
using Monitoring;
using PatientService.Data;
using PatientService.Models;
using PatientService.Repositories.Interfaces;

namespace PatientService.Repositories
{
    public class PatientRepository(PatientDbContext context) : IPatientRepository
    {
        public async Task<IEnumerable<Patient?>> GetAllPatientsAsync()
        {
            using var activity = LoggingService.activitySource.StartActivity();

            return await context.Patients.ToListAsync();
        }

        public async Task<Patient?> GetPatientByIdAsync(Guid patientId)
        {
            using var activity = LoggingService.activitySource.StartActivity();

            return await context.Patients.FindAsync(patientId);
        }

        public async Task AddPatientAsync(Patient? patient)
        {
            using var activity = LoggingService.activitySource.StartActivity();

            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePatientAsync(Patient? patient)
        {
            using var activity = LoggingService.activitySource.StartActivity();

            context.Patients.Update(patient);
            await context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(Guid patientId)
        {
            using var activity = LoggingService.activitySource.StartActivity();

            var patient = await context.Patients.FindAsync(patientId);
            if (patient != null)
            {
                context.Patients.Remove(patient);
                await context.SaveChangesAsync();
            }
        }
    }
}