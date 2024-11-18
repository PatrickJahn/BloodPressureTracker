using Microsoft.AspNetCore.Mvc;
using MeasurementService.Services.Interfaces;
using MeasurementService.DTOs;
using Microsoft.FeatureManagement;

namespace MeasurementService.Controllers
{
    [ApiController]
    [Route("api/[controller]/{regionCode}")]
    public class MeasurementsController(IMeasurementService measurementService, IFeatureManager featureManager, IConfiguration configuration)
        : ControllerBase
    {
        private readonly string? _serviceRegion = configuration.GetValue<string>("RegionCode");

        private ActionResult ValidateRegionCode(string regionCode)
        {
            return !_serviceRegion.Equals(regionCode, StringComparison.OrdinalIgnoreCase) ? BadRequest(new { Message = $"Invalid region code '{regionCode}' for this service." }) : null;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeasurementDto>>> GetMeasurements(string regionCode)
        {
            var validationResult = ValidateRegionCode(regionCode);

            if (validationResult != null) return validationResult;

            if (!await featureManager.IsEnabledAsync("EnableGetAllMeasurements"))
            {
                return StatusCode(503, "The feature to retrieve all measurements is currently disabled.");
            }

            var measurements = await measurementService.GetAllMeasurementsAsync();
            return Ok(measurements);
        }

        [HttpGet("{ssn}")]
        public async Task<ActionResult<IEnumerable<MeasurementDto>>> GetMeasurementsByPatientSsn(string regionCode, string ssn)
        {
            var validationResult = ValidateRegionCode(regionCode);

            if (validationResult != null) return validationResult;

            if (!await featureManager.IsEnabledAsync("EnableGetMeasurementsBySSN"))
            {
                return StatusCode(503, "The feature to retrieve measurements by SSN is currently disabled.");
            }

            var measurements = await measurementService.GetMeasurementsBySSNAsync(ssn);
            return Ok(measurements);
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateMeasurement(string regionCode, [FromBody] CreateMeasurementDto measurement)
        {
            var validationResult = ValidateRegionCode(regionCode);

            if (validationResult != null) return validationResult;

            if (!await featureManager.IsEnabledAsync("EnableCreateMeasurement"))
            {
                return StatusCode(503, "The feature to create a measurement is currently disabled.");
            }

            await measurementService.AddMeasurementAsync(measurement);
            return CreatedAtAction(nameof(GetMeasurements), new { regionCode }, measurement);
        }
    }
}
