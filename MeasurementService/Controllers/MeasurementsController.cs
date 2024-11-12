using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MeasurementService.Models;

namespace MeasurementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementsController : ControllerBase
    {
         private readonly IHttpClientFactory _clientFactory;

    public MeasurementsController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurements()
    {
        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync("http://your-database-endpoint/measurements");

        if (response.IsSuccessStatusCode)
        {
            var measurements = await response.Content.ReadAsAsync<IEnumerable<Measurement>>();
            return Ok(measurements);
        }

        return StatusCode(500, "Error retrieving measurements.");
    }
     [HttpPost]
    public async Task<ActionResult> CreateMeasurement([FromBody] Measurement measurement)
    {
        var client = _clientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("http://your-database-endpoint/measurements", measurement);

        if (response.IsSuccessStatusCode)
        {
            return CreatedAtAction(nameof(GetMeasurements), new { id = measurement.Id }, measurement);
        }

        return BadRequest("Error creating measurement.");
        
    }
    
}
}