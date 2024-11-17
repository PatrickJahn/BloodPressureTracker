using Microsoft.AspNetCore.Mvc;
using MeasurementService.Models;
using MeasurementService.Services.Interfaces;
using MeasurementService.DTOs;
using Microsoft.FeatureManagement;

namespace MeasurementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementsController(IMeasurementService measurementService, IFeatureManager featureManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurements()
        {
            if (!await featureManager.IsEnabledAsync("EnableGetAllMeasurements"))
            {
                return StatusCode(503, "The feature to retrieve all measurements is currently disabled.");
            }

            var measurements = await measurementService.GetAllMeasurementsAsync();
            return Ok(measurements);
        }

        [HttpGet("{ssn}")]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurementsByPatientSSn(string ssn)
        {
            if (!await featureManager.IsEnabledAsync("EnableGetMeasurementsBySSN"))
            {
                return StatusCode(503, "The feature to retrieve measurements by SSN is currently disabled.");
            }

            var measurements = await measurementService.GetMeasurementsBySSn(ssn);
            return Ok(measurements);
        }

        [HttpPost]
        public async Task<ActionResult> CreateMeasurement([FromBody] CreateMeasurementDto measurement)
        {
            if (!await featureManager.IsEnabledAsync("EnableCreateMeasurement"))
            {
                return StatusCode(503, "The feature to create a measurement is currently disabled.");
            }

            await measurementService.AddMeasurementAsync(measurement);
            return Ok();
        }
    }
}