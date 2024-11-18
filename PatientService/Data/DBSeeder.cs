using PatientService.Models;

namespace PatientService.Data
{
    public abstract class DbSeeder
    {
        public static void Seed(PatientDbContext dbContext)
        {
            if (!dbContext.Patients.Any())
            {
                dbContext.Patients.AddRange(
                    new Patient
                    {
                        PatientId = Guid.NewGuid(),
                        SSN = "123-45-6789",
                        Name = "John Doe",
                        Email = "johndoe@example.com",
                        DateOfBirth = new DateTime(1990, 1, 1),
                        Gender = "Male",
                        MedicalHistory = "No known allergies.",
                        RegionCode = "Zealand"
                    },
                    new Patient
                    {
                        PatientId = Guid.NewGuid(),
                        SSN = "987-65-4321",
                        Name = "Jane Smith",
                        Email = "janesmith@example.com",
                        DateOfBirth = new DateTime(1985, 5, 20),
                        Gender = "Female",
                        MedicalHistory = "Diabetic, requires insulin injections.",
                        RegionCode = "Midtjylland"
                    }
                );
                dbContext.SaveChanges();
            }
        }
    }
}