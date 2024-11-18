using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using PatientService.DTOs;
using PatientService.Services.Interfaces;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("api/{regionCode}/[controller]")]
    public class PatientController(IPatientService patientService, IFeatureManager featureManager, IConfiguration configuration)
        : ControllerBase
    {
        private readonly string? _serviceRegion = configuration.GetValue<string>("RegionCode");

        private ActionResult ValidateRegionCode(string regionCode)
        {
            return !_serviceRegion.Equals(regionCode, StringComparison.OrdinalIgnoreCase) ? BadRequest(new { Message = $"Invalid region code '{regionCode}' for this service." }) : null;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAllPatients(string regionCode)
        {
            var validationResult = ValidateRegionCode(regionCode);
            if (validationResult != null) return validationResult;

            if (!await featureManager.IsEnabledAsync("EnableGetAllPatients"))
            {
                return StatusCode(503, "The feature to retrieve all patients is currently disabled.");
            }

            var patients = await patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatientDto>> GetPatientById(string regionCode, Guid id)
        {
            var validationResult = ValidateRegionCode(regionCode);
            if (validationResult != null) return validationResult;

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

        [HttpPost("")]
        public async Task<ActionResult> CreatePatient(string regionCode, [FromBody] CreatePatientDto patientDto)
        {
            var validationResult = ValidateRegionCode(regionCode);
            if (validationResult != null) return validationResult;

            if (!await featureManager.IsEnabledAsync("EnableCreatePatient"))
            {
                return StatusCode(503, "The feature to create a patient is currently disabled.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await patientService.AddPatientAsync(patientDto);
            return CreatedAtAction(nameof(GetPatientById), new { regionCode, id = Guid.NewGuid() }, patientDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdatePatient(string regionCode, Guid id, [FromBody] UpdatePatientDto patientDto)
        {
            var validationResult = ValidateRegionCode(regionCode);
            if (validationResult != null) return validationResult;

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
        public async Task<ActionResult> DeletePatient(string regionCode, Guid id)
        {
            var validationResult = ValidateRegionCode(regionCode);
            if (validationResult != null) return validationResult;

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
