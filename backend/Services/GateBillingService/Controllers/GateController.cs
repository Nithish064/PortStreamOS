using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortStream.Shared.Models;
using GateBillingService.Data;
using GateBillingService.Services;

namespace GateBillingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GateController : ControllerBase
{
    private readonly GateBillingDbContext _context;
    private readonly KafkaProducerService _kafkaProducer;

    public GateController(GateBillingDbContext context, KafkaProducerService kafkaProducer)
    {
        _context = context;
        _kafkaProducer = kafkaProducer;
    }

    [HttpPost("event")]
    public async Task<IActionResult> LogGateEvent([FromBody] GateEvent gateEvent)
    {
        gateEvent.Timestamp = DateTime.UtcNow;

        // Save to Database
        _context.GateEvents.Add(gateEvent);
        await _context.SaveChangesAsync();

        // Publish to Kafka
        await _kafkaProducer.PublishGateEventAsync(gateEvent);

        return Ok(gateEvent);
    }

    [HttpGet]
    public async Task<IActionResult> GetRecentEvents()
    {
        var events = await _context.GateEvents.OrderByDescending(e => e.Timestamp).Take(50).ToListAsync();
        return Ok(events);
    }
}
