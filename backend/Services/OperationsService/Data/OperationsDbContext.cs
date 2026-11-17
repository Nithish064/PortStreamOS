using Microsoft.EntityFrameworkCore;
using PortStream.Shared.Models;

namespace OperationsService.Data;

public class OperationsDbContext : DbContext
{
    public OperationsDbContext(DbContextOptions<OperationsDbContext> options) : base(options)
    {
    }

    public DbSet<EnvironmentReading> EnvironmentReadings { get; set; }
    public DbSet<WorkerShift> WorkerShifts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Seed some dummy shifts for the UI
        modelBuilder.Entity<WorkerShift>().HasData(
            new WorkerShift { Id = Guid.NewGuid(), WorkerName = "John Doe", Role = "Crane Operator", StartTime = DateTime.UtcNow.AddHours(-10), EndTime = DateTime.UtcNow.AddHours(2), SafetyGearChecked = true },
            new WorkerShift { Id = Guid.NewGuid(), WorkerName = "Jane Smith", Role = "Stevedore", StartTime = DateTime.UtcNow.AddHours(-14), EndTime = DateTime.UtcNow.AddHours(-2), SafetyGearChecked = true } // Flagged! > 12 hours
        );
    }
}
