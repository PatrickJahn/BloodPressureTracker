using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MeasurementService.Models;
using MeasurementService.Services.Interfaces;
using MeasurementService.DTOs;

namespace MeasurementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementsController : ControllerBase
    {
         private readonly IMeasurementService _measurementService;

    public MeasurementsController(IMeasurementService clientFactory)
    {
        _measurementService = clientFactory;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurements()
    {
        var measurements = await _measurementService.GetAllMeasurementsAsync();

        return Ok(measurements);
    }
    
    [HttpGet("{ssn}")]
    public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurementsByPatientSSn(string ssn)
    {
        var measurements = await _measurementService.GetMeasurementsBySSn(ssn);
        return Ok(measurements);
    }

    
     [HttpPost]
    public async Task<ActionResult> CreateMeasurement([FromBody] CreateMeasurementDto measurement)
    {
        await _measurementService.AddMeasurementAsync(measurement);

    return Ok();    
    }
    
}
}