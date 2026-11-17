using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortStream.Shared.Models;
using CustomsRiskService.Data;

namespace CustomsRiskService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RiskRuleController : ControllerBase
{
    private readonly CustomsRiskDbContext _context;

    public RiskRuleController(CustomsRiskDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRules()
    {
        var rules = await _context.RiskRules.ToListAsync();
        return Ok(rules);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRule(RiskRule rule)
    {
        _context.RiskRules.Add(rule);
        await _context.SaveChangesAsync();
        return Ok(rule);
    }
}
