using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using PatientService.DTOs;
using PatientService.Services.Interfaces;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController(IPatientService patientService, IFeatureManager featureManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAllPatients()
        {
            if (!await featureManager.IsEnabledAsync("EnableGetAllPatients"))
            {
                return StatusCode(503, "The feature to retrieve all patients is currently disabled.");
            }

            var patients = await patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatientDto>> GetPatientById(Guid id)
        {
            if (!await featureManager.IsEnabledAsync("EnableGetPatientById"))
            {
                return StatusCode(503, "The feature to retrieve a patient by ID is currently disabled.");
            }

            var patient = await patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound(new { Message = "Patient not found" });
            }
            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePatient([FromBody] CreatePatientDto patientDto)
        {
            if (!await featureManager.IsEnabledAsync("EnableCreatePatient"))
            {
                return StatusCode(503, "The feature to create a patient is currently disabled.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await patientService.AddPatientAsync(patientDto);
            return CreatedAtAction(nameof(GetPatientById), new { id = new Guid() }, patientDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientDto patientDto)
        {
            if (!await featureManager.IsEnabledAsync("EnableUpdatePatient"))
            {
                return StatusCode(503, "The feature to update a patient is currently disabled.");
            }

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

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeletePatient(Guid id)
        {
            if (!await featureManager.IsEnabledAsync("EnableDeletePatient"))
            {
                return StatusCode(503, "The feature to delete a patient is currently disabled.");
            }

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
