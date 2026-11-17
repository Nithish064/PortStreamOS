using Microsoft.EntityFrameworkCore;
using PortStream.Shared.Models;

namespace YardEquipmentService.Data;

public class YardEquipmentDbContext : DbContext
{
    public YardEquipmentDbContext(DbContextOptions<YardEquipmentDbContext> options) : base(options)
    {
    }

    public DbSet<YardBin> YardBins { get; set; }
    public DbSet<Container> Containers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Seed initial yard blocks for the Heat-Matrix
        modelBuilder.Entity<YardBin>().HasData(
            new YardBin { Id = Guid.NewGuid(), BlockName = "Block A", MaxCapacity = 500, CurrentCount = 450 }, // 90% full
            new YardBin { Id = Guid.NewGuid(), BlockName = "Block B", MaxCapacity = 500, CurrentCount = 200 }, // 40% full
            new YardBin { Id = Guid.NewGuid(), BlockName = "Block C", MaxCapacity = 500, CurrentCount = 490 }  // 98% full (Critical)
        );
    }
}
