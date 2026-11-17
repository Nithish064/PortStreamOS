using Microsoft.EntityFrameworkCore;
using Serilog;
using StackExchange.Redis;
using YardEquipmentService.Data;
using YardEquipmentService.Services;

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
builder.Services.AddDbContext<YardEquipmentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") 
                      ?? "Host=localhost;Database=portstream_yard_db;Username=admin;Password=password"));

// Configure Redis Multiplexer
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => 
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379"));

// Register Services
builder.Services.AddScoped<YardCacheService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Auto-migrate in development for demo
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<YardEquipmentDbContext>();
    db.Database.EnsureCreated();
}

app.UseAuthorization();
app.MapControllers();

// Run on a different port than VesselTrafficService (which is 5001)
app.Run("http://localhost:5002");
