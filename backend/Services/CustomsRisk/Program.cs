using Microsoft.EntityFrameworkCore;
using Serilog;
using CustomsRiskService.Data;
using CustomsRiskService.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration)
                 .Enrich.FromLogContext()
                 .WriteTo.Console());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure PostgreSQL DbContext
builder.Services.AddDbContext<CustomsRiskDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") 
                      ?? "Host=localhost;Database=portstream_customs_db;Username=admin;Password=password"));

// Register Services
builder.Services.AddScoped<RiskAssessmentService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Auto-migrate
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<CustomsRiskDbContext>();
    db.Database.EnsureCreated();
}

app.UseAuthorization();
app.MapControllers();

// Run on Port 5003
app.Run("http://localhost:5003");
