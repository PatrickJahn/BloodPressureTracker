using Microsoft.AspNetCore.Mvc;
using PatientService.DTOs;
using PatientService.Services.Interfaces;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController(IPatientService patientService) : ControllerBase
    {
        // GET: api/patient
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAllPatients()
        {
            var patients = await patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        // GET: api/patient/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatientDto>> GetPatientById(Guid id)
        {
            var patient = await patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound(new { Message = "Patient not found" });
            }
            return Ok(patient);
        }

        // POST: api/patient
        [HttpPost]
        public async Task<ActionResult> CreatePatient([FromBody] CreatePatientDto patientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await patientService.AddPatientAsync(patientDto);
            return CreatedAtAction(nameof(GetPatientById), new { id = new Guid() }, patientDto);
        }

        // PUT: api/patient/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientDto patientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = await patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound(new { Message = "Patient not found" });
            }

            await patientService.UpdatePatientAsync(id, patientDto);
            return NoContent();
        }

        // DELETE: api/patient/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeletePatient(Guid id)
        {
            var patient = await patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound(new { Message = "Patient not found" });
            }

            await patientService.DeletePatientAsync(id);
            return NoContent();
        }
    }
}
