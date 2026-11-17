using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortStream.Shared.Models;
using CustomsRiskService.Data;
using CustomsRiskService.Services;

namespace CustomsRiskService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManifestController : ControllerBase
{
    private readonly CustomsRiskDbContext _context;
    private readonly RiskAssessmentService _riskService;

    public ManifestController(CustomsRiskDbContext context, RiskAssessmentService riskService)
    {
        _context = context;
        _riskService = riskService;
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitManifest([FromBody] CustomsManifest manifest)
    {
        // 1. Fetch active rules
        var rules = await _context.RiskRules.Where(r => r.IsActive).ToListAsync();

        // 2. Evaluate
        var evaluatedManifest = _riskService.EvaluateManifest(manifest, rules);

        // 3. Save to DB
        _context.Manifests.Add(evaluatedManifest);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetManifest), new { id = evaluatedManifest.Id }, evaluatedManifest);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllManifests()
    {
        var manifests = await _context.Manifests.Include(m => m.CargoItems).ToListAsync();
        return Ok(manifests);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetManifest(Guid id)
    {
        var manifest = await _context.Manifests.Include(m => m.CargoItems).FirstOrDefaultAsync(m => m.Id == id);
        if (manifest == null) return NotFound();
        return Ok(manifest);
    }
}
