using Microsoft.EntityFrameworkCore;
using PortStream.Shared.Models;

namespace CustomsRiskService.Data;

public class CustomsRiskDbContext : DbContext
{
    public CustomsRiskDbContext(DbContextOptions<CustomsRiskDbContext> options) : base(options)
    {
    }

    public DbSet<CustomsManifest> Manifests { get; set; }
    public DbSet<CargoItem> CargoItems { get; set; }
    public DbSet<RiskRule> RiskRules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<CustomsManifest>()
            .HasMany(m => m.CargoItems);

        // Seed some initial compliance rules
        modelBuilder.Entity<RiskRule>().HasData(
            new RiskRule { Id = Guid.NewGuid(), RuleName = "High Risk Origin", TargetField = "OriginCountry", Operator = "Equals", Value = "CountryX", AssignedRiskLevel = RiskLevel.High, IsActive = true },
            new RiskRule { Id = Guid.NewGuid(), RuleName = "Heavy Cargo Audit", TargetField = "WeightKg", Operator = "GreaterThan", Value = "50000", AssignedRiskLevel = RiskLevel.Medium, IsActive = true }
        );
    }
}
