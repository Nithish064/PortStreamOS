using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortStream.Shared.Models;
using OperationsService.Data;

namespace OperationsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkforceController : ControllerBase
{
    private readonly OperationsDbContext _context;

    public WorkforceController(OperationsDbContext context)
    {
        _context = context;
    }

    [HttpGet("active-shifts")]
    public async Task<IActionResult> GetActiveShifts()
    {
        var shifts = await _context.WorkerShifts
            .OrderBy(s => s.StartTime)
            .ToListAsync();
            
        return Ok(shifts);
    }

    [HttpPost("start-shift")]
    public async Task<IActionResult> StartShift([FromBody] WorkerShift shift)
    {
        shift.StartTime = DateTime.UtcNow;
        _context.WorkerShifts.Add(shift);
        await _context.SaveChangesAsync();
        return Ok(shift);
    }
}
