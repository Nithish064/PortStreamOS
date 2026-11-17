using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YardEquipmentService.Data;
using YardEquipmentService.Services;

namespace YardEquipmentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class YardController : ControllerBase
{
    private readonly YardEquipmentDbContext _context;
    private readonly YardCacheService _cache;
    private readonly ILogger<YardController> _logger;

    public YardController(YardEquipmentDbContext context, YardCacheService cache, ILogger<YardController> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    [HttpGet("heatmap")]
    public async Task<IActionResult> GetHeatmap()
    {
        // 1. Try to get fast data from Redis Cache
        var cachedData = await _cache.GetYardHeatmapAsync();
        if (cachedData != null)
        {
            _logger.LogInformation("Serving heatmap from Redis cache.");
            return Ok(cachedData);
        }

        // 2. Cache miss -> Fetch from PostgreSQL
        _logger.LogInformation("Fetching heatmap from Postgres Database.");
        var dbData = await _context.YardBins.ToListAsync();
        
        // 3. Update Cache
        await _cache.CacheYardHeatmapAsync(dbData);

        return Ok(dbData);
    }
}
