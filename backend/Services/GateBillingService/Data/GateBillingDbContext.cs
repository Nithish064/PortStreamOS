using Microsoft.EntityFrameworkCore;
using PortStream.Shared.Models;

namespace GateBillingService.Data;

public class GateBillingDbContext : DbContext
{
    public GateBillingDbContext(DbContextOptions<GateBillingDbContext> options) : base(options)
    {
    }

    public DbSet<GateEvent> GateEvents { get; set; }
    public DbSet<Invoice> Invoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
