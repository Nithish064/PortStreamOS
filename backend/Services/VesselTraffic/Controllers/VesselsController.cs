using Microsoft.AspNetCore.Mvc;
using PortStream.Shared.Models;
using VesselTrafficService.Repositories;

namespace VesselTrafficService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VesselsController : ControllerBase
{
    private readonly IVesselRepository _repository;
    private readonly ILogger<VesselsController> _logger;

    public VesselsController(IVesselRepository repository, ILogger<VesselsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetVessels()
    {
        _logger.LogInformation("Fetching all vessels.");
        var vessels = await _repository.GetAllVesselsAsync();
        return Ok(vessels);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVessel(Guid id)
    {
        var vessel = await _repository.GetVesselByIdAsync(id);
        if (vessel == null)
        {
            _logger.LogWarning("Vessel with id {Id} not found.", id);
            return NotFound();
        }
        return Ok(vessel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateVessel(Vessel vessel)
    {
        var created = await _repository.AddVesselAsync(vessel);
        _logger.LogInformation("Vessel created: {Id}", created.Id);
        return CreatedAtAction(nameof(GetVessel), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVessel(Guid id, Vessel vessel)
    {
        if (id != vessel.Id) return BadRequest("ID mismatch");

        var updated = await _repository.UpdateVesselAsync(vessel);
        if (updated == null) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVessel(Guid id)
    {
        var deleted = await _repository.DeleteVesselAsync(id);
        if (!deleted) return NotFound();
        
        _logger.LogInformation("Vessel deleted: {Id}", id);
        return NoContent();
    }
}
