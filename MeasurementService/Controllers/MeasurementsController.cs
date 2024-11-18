using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MeasurementService.Services.Interfaces;
using MeasurementService.DTOs;
using Microsoft.FeatureManagement;
using Monitoring;

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
            
            var parentContext = ActivityHelper.ExtractPropagationContextFromHttpRequest(Request);
            using var activity = LoggingService.activitySource.StartActivity("Get All Measurements requested", ActivityKind.Consumer, parentContext.ActivityContext);
            LoggingService.Log.AddContext().Information($"Get Measurements endpoint called");

            if (!await featureManager.IsEnabledAsync("EnableGetAllMeasurements"))
            {
                LoggingService.Log.AddContext().Information($"Get Measurements endpoint is disabled");
                return StatusCode(503, "The feature to retrieve all measurements is currently disabled.");
            }

            var measurements = await measurementService.GetAllMeasurementsAsync();
            
            LoggingService.Log.AddContext().Information($"Measurements retrieved successfully");

            return Ok(measurements);
        }

        [HttpGet("{ssn}")]
        public async Task<ActionResult<IEnumerable<MeasurementDto>>> GetMeasurementsByPatientSsn(string regionCode, string ssn)
        {
            var validationResult = ValidateRegionCode(regionCode);

            if (validationResult != null) return validationResult;

            var parentContext = ActivityHelper.ExtractPropagationContextFromHttpRequest(Request);
            using var activity = LoggingService.activitySource.StartActivity("Get All Measurements by SSn called", ActivityKind.Consumer, parentContext.ActivityContext);
            LoggingService.Log.AddContext().Information($"Get Measurements by SSn endpoint called");

            if (!await featureManager.IsEnabledAsync("EnableGetMeasurementsBySSN"))
            {
                LoggingService.Log.AddContext().Information($"Get Measurements by SSn endpoint is disabled");

                return StatusCode(503, "The feature to retrieve measurements by SSN is currently disabled.");
            }

            var measurements = await measurementService.GetMeasurementsBySSNAsync(ssn);
            LoggingService.Log.AddContext().Information($"Measurements retrieved successfully");

            return Ok(measurements);
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateMeasurement(string regionCode, [FromBody] CreateMeasurementDto measurement)
        {
            var validationResult = ValidateRegionCode(regionCode);

            if (validationResult != null) return validationResult;

            var parentContext = ActivityHelper.ExtractPropagationContextFromHttpRequest(Request);
            using var activity = LoggingService.activitySource.StartActivity("Create measurement endpoint called", ActivityKind.Consumer, parentContext.ActivityContext);
            LoggingService.Log.AddContext().Information($"Create measurement endpoint called");
            
            if (!await featureManager.IsEnabledAsync("EnableCreateMeasurement"))
            {
                LoggingService.Log.AddContext().Information($"Create measurement endpoint is disabled");
                return StatusCode(503, "The feature to create a measurement is currently disabled.");
            }

            await measurementService.AddMeasurementAsync(measurement);
            LoggingService.Log.AddContext().Information($"Measurement added successfully");

            return CreatedAtAction(nameof(GetMeasurements), new { regionCode }, measurement);
        }
    }
}
