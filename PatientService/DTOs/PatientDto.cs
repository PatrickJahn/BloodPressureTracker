namespace PatientService.DTOs
{
    public class PatientDto
    {
        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string SSN { get; set; }
    }

    public class CreatePatientDto
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ContactInfo { get; set; }
        public string Email { get; set; }
        public string SSN { get; set; }
    }

    public class UpdatePatientDto
    {
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public string MedicalHistory { get; set; }
        public string Email { get; set; }
        public string SSN { get; set; }
    }
}