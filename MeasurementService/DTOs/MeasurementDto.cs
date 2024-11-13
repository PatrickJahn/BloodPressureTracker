namespace MeasurementService.DTOs
{
    public class MeasurementDto
    {
        public int Id { get; set; }           // Unique identifier for the measurement
        public DateTime Date { get; set; }    // Date and time the measurement was taken
        public int Systolic { get; set; }     // Systolic blood pressure value
        public int Diastolic { get; set; }    // Diastolic blood pressure value
        public bool Seen { get; set; }  
    }

    public class CreateMeasurementDto
    {
          public int Systolic { get; set; }     // Systolic blood pressure value
          public int Diastolic { get; set; }    // Diastolic blood pressure value
    }

}

