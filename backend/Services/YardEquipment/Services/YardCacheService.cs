using PortStream.Shared.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace YardEquipmentService.Services;

public class YardCacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<YardCacheService> _logger;

    public YardCacheService(IConnectionMultiplexer redis, ILogger<YardCacheService> logger)
    {
        _redis = redis;
        _logger = logger;
    }

    public async Task CacheYardHeatmapAsync(IEnumerable<YardBin> bins)
    {
        var db = _redis.GetDatabase();
        var serializedData = JsonSerializer.Serialize(bins);
        
        // Cache the heatmap data for 1 minute (simulating high-frequency updates)
        await db.StringSetAsync("yard:heatmap", serializedData, TimeSpan.FromMinutes(1));
        _logger.LogInformation("Yard heatmap cached in Redis.");
    }

    public async Task<IEnumerable<YardBin>?> GetYardHeatmapAsync()
    {
        var db = _redis.GetDatabase();
        var data = await db.StringGetAsync("yard:heatmap");
        
        if (data.IsNullOrEmpty)
        {
            _logger.LogWarning("Yard heatmap cache miss.");
            return null;
        }

        return JsonSerializer.Deserialize<IEnumerable<YardBin>>((string)data!);
    }
}
