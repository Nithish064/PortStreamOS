using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GateBillingService.Data;

namespace GateBillingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillingController : ControllerBase
{
    private readonly GateBillingDbContext _context;

    public BillingController(GateBillingDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetInvoices()
    {
        var invoices = await _context.Invoices.OrderByDescending(i => i.IssueDate).ToListAsync();
        return Ok(invoices);
    }
}
