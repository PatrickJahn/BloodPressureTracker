using System;
using System.ComponentModel.DataAnnotations;

namespace PatientService.Models
{
    public class Patient
    {
        [Key]
        public Guid PatientId { get; set; }
        [MaxLength(20)]
        public string RegionCode { get; set; }
        
        [Required]
        public string SSN { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public required DateTime DateOfBirth { get; set; }
        [Required]
        public required string Gender { get; set; }

        public string? MedicalHistory { get; set; }
    }
}