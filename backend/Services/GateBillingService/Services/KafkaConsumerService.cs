using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PortStream.Shared.Models;
using GateBillingService.Data;

namespace GateBillingService.Services;

public class KafkaConsumerService : BackgroundService
{
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _bootstrapServers;

    public KafkaConsumerService(ILogger<KafkaConsumerService> logger, IServiceScopeFactory scopeFactory, IConfiguration config)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _bootstrapServers = config.GetConnectionString("Kafka") ?? "localhost:9092";
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
    }

    private void StartConsumerLoop(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = "billing-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("truck-gate-events");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(stoppingToken);
                if (consumeResult != null)
                {
                    var gateEvent = JsonSerializer.Deserialize<GateEvent>(consumeResult.Message.Value);
                    if (gateEvent != null)
                    {
                        ProcessEvent(gateEvent).Wait();
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Expected gracefully
        }
        finally
        {
            consumer.Close();
        }
    }

    private async Task ProcessEvent(GateEvent gateEvent)
    {
        _logger.LogInformation("Consumed GateEvent from Kafka: Truck {TruckPlate} - {Type}", gateEvent.TruckLicensePlate, gateEvent.Type);

        if (gateEvent.Type == EventType.Exit)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<GateBillingDbContext>();
            
            // Calculate a dummy tariff (e.g., $50 base fee)
            var invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(1000, 9999)}",
                TruckLicensePlate = gateEvent.TruckLicensePlate,
                IssueDate = DateTime.UtcNow,
                TotalAmount = 50.00, 
                Reason = "Port Tariff - Truck Gate Exit"
            };

            db.Invoices.Add(invoice);
            await db.SaveChangesAsync();

            _logger.LogInformation("Generated Invoice {InvoiceNumber} for Truck {TruckPlate}", invoice.InvoiceNumber, invoice.TruckLicensePlate);
        }
    }
}
