using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MeasurementService.Models;
using MeasurementService.Services.Interfaces;
using MeasurementService.DTOs;
using Microsoft.FeatureManagement;
using Monitoring;

namespace MeasurementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementsController(IMeasurementService measurementService, IFeatureManager featureManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurements()
        {
            
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
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurementsByPatientSSn(string ssn)
        {
            var parentContext = ActivityHelper.ExtractPropagationContextFromHttpRequest(Request);
            using var activity = LoggingService.activitySource.StartActivity("Get All Measurements by SSn called", ActivityKind.Consumer, parentContext.ActivityContext);
            LoggingService.Log.AddContext().Information($"Get Measurements by SSn endpoint called");

            if (!await featureManager.IsEnabledAsync("EnableGetMeasurementsBySSN"))
            {
                LoggingService.Log.AddContext().Information($"Get Measurements by SSn endpoint is disabled");

                return StatusCode(503, "The feature to retrieve measurements by SSN is currently disabled.");
            }

            var measurements = await measurementService.GetMeasurementsBySSn(ssn);
            LoggingService.Log.AddContext().Information($"Measurements retrieved successfully");

            return Ok(measurements);
        }

        [HttpPost]
        public async Task<ActionResult> CreateMeasurement([FromBody] CreateMeasurementDto measurement)
        {
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

            return Ok();
        }
    }
}