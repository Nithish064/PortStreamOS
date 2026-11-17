using System.Text.Json;
using Confluent.Kafka;
using PortStream.Shared.Models;

namespace GateBillingService.Services;

public class KafkaProducerService
{
    private readonly ILogger<KafkaProducerService> _logger;
    private readonly IProducer<Null, string> _producer;

    public KafkaProducerService(IConfiguration config, ILogger<KafkaProducerService> logger)
    {
        _logger = logger;
        
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config.GetConnectionString("Kafka") ?? "localhost:9092"
        };
        
        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    }

    public async Task PublishGateEventAsync(GateEvent gateEvent)
    {
        var message = JsonSerializer.Serialize(gateEvent);
        
        _logger.LogInformation("Publishing GateEvent {EventType} for Truck {TruckPlate} to Kafka topic 'truck-gate-events'", gateEvent.Type, gateEvent.TruckLicensePlate);

        var result = await _producer.ProduceAsync("truck-gate-events", new Message<Null, string> { Value = message });
        
        _logger.LogInformation("Message published at offset {Offset}", result.Offset);
    }
}
