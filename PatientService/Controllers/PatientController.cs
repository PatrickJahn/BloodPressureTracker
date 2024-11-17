using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Monitoring;
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
            var parentContext = ActivityHelper.ExtractPropagationContextFromHttpRequest(Request);
            using var activity = LoggingService.activitySource.StartActivity("Get All Patients requested", ActivityKind.Consumer, parentContext.ActivityContext);
            LoggingService.Log.AddContext().Information($"Get all Patients endpoint called");

            if (!await featureManager.IsEnabledAsync("EnableGetAllPatients"))
            {
                LoggingService.Log.AddContext().Information($"Get all Patients endpoint disabled");

                return StatusCode(503, "The feature to retrieve all patients is currently disabled.");
            }

            var patients = await patientService.GetAllPatientsAsync();
            
            LoggingService.Log.AddContext().Information($"Patients retrieved successfully");

            return Ok(patients);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatientDto>> GetPatientById(Guid id)
        {
            var parentContext = ActivityHelper.ExtractPropagationContextFromHttpRequest(Request);
            using var activity = LoggingService.activitySource.StartActivity("Get patient by id reuested", ActivityKind.Consumer, parentContext.ActivityContext);
            LoggingService.Log.AddContext().Information($"Get patient by id endpoint called with id: {id.ToString()}");

            if (!await featureManager.IsEnabledAsync("EnableGetPatientById"))
            {
                LoggingService.Log.AddContext().Information($"Get patient by id endpoint is disabled");

                return StatusCode(503, "The feature to retrieve a patient by ID is currently disabled.");
            }

            var patient = await patientService.GetPatientByIdAsync(id);
            
            
            if (patient == null)
            {
                
                LoggingService.Log.AddContext().Information($"Patient with id {id.ToString()} was not found");
                return NotFound(new { Message = "Patient not found" });
            }
            
            LoggingService.Log.AddContext().Information($"Patient with id {id.ToString()} was returned successfully");

            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePatient([FromBody] CreatePatientDto patientDto)
        {
            var parentContext = ActivityHelper.ExtractPropagationContextFromHttpRequest(Request);
            using var activity = LoggingService.activitySource.StartActivity("Create patient endpoint ", ActivityKind.Consumer, parentContext.ActivityContext);
            LoggingService.Log.AddContext().Information($"Create patient endpoint was called with value: {JsonSerializer.Serialize(patientDto)}");

            
            if (!await featureManager.IsEnabledAsync("EnableCreatePatient"))
            {
                LoggingService.Log.AddContext().Information($"Create patient endpoint is disabled");

                return StatusCode(503, "The feature to create a patient is currently disabled.");
            }

            if (!ModelState.IsValid)
            {
                LoggingService.Log.AddContext().Information($"Model state was not vaild");
                return BadRequest(ModelState);
            }

            await patientService.AddPatientAsync(patientDto);
            
            LoggingService.Log.AddContext().Information($"Patient was created successfully");

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
