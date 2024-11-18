using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeasurementService.Models
{
    public class Measurement
    {
        public int Id { get; set; }           // Unique identifier for the measurement
        public DateTime Date { get; set; }    // Date and time the measurement was taken
        public int Systolic { get; set; }     // Systolic blood pressure value
        public int Diastolic { get; set; }    // Diastolic blood pressure value
        public bool Seen { get; set; }        // Flag to indicate if the measurement was seen/reviewed
        
        public string PatientSSN { get; set; }
     
    }
}