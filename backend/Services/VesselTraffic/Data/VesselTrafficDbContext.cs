using Microsoft.EntityFrameworkCore;
using PortStream.Shared.Models;

namespace VesselTrafficService.Data;

public class VesselTrafficDbContext : DbContext
{
    public VesselTrafficDbContext(DbContextOptions<VesselTrafficDbContext> options) : base(options)
    {
    }

    public DbSet<Vessel> Vessels { get; set; }
    public DbSet<Berth> Berths { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Vessel>()
            .HasKey(v => v.Id);
            
        modelBuilder.Entity<Berth>()
            .HasKey(b => b.Id);
            
        // Initial Seed Data
        modelBuilder.Entity<Berth>().HasData(
            new Berth { Id = Guid.NewGuid(), Name = "Berth A1", MaxVesselLength = 400, MaxDraft = 15, Status = BerthStatus.Available },
            new Berth { Id = Guid.NewGuid(), Name = "Berth A2", MaxVesselLength = 300, MaxDraft = 12, Status = BerthStatus.Available }
        );
    }
}
