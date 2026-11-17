using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortStream.Shared.Models;
using OperationsService.Data;

namespace OperationsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnvironmentController : ControllerBase
{
    private readonly OperationsDbContext _context;

    public EnvironmentController(OperationsDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> LogReading([FromBody] EnvironmentReading reading)
    {
        reading.Timestamp = DateTime.UtcNow;
        _context.EnvironmentReadings.Add(reading);
        await _context.SaveChangesAsync();
        return Ok(reading);
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestReadings()
    {
        // Get the latest reading per sensor location
        var readings = await _context.EnvironmentReadings
            .GroupBy(r => r.SensorLocation)
            .Select(g => g.OrderByDescending(r => r.Timestamp).FirstOrDefault())
            .ToListAsync();
            
        return Ok(readings);
    }
}
