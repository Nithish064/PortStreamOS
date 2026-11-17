using System.Text.Json;
using Confluent.Kafka;
using PortStream.Shared.Models;

namespace NotificationService.Services;

public class KafkaNotificationConsumer : BackgroundService
{
    private readonly ILogger<KafkaNotificationConsumer> _logger;
    private readonly SmtpEmailSender _emailSender;
    private readonly string _bootstrapServers;

    public KafkaNotificationConsumer(ILogger<KafkaNotificationConsumer> logger, SmtpEmailSender emailSender, IConfiguration config)
    {
        _logger = logger;
        _emailSender = emailSender;
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
            GroupId = "notification-consumer-group",
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
            // Expected
        }
        finally
        {
            consumer.Close();
        }
    }

    private async Task ProcessEvent(GateEvent gateEvent)
    {
        if (gateEvent.Type == EventType.Exit)
        {
            _logger.LogInformation("Notification Consumer caught Exit Event for Truck {TruckPlate}", gateEvent.TruckLicensePlate);
            
            // Format HTML Email
            string emailHtml = $@"
                <h2>PortStream OS - Gate Exit Receipt</h2>
                <p>Hello,</p>
                <p>This is an automated notification that Truck <strong>{gateEvent.TruckLicensePlate}</strong> has exited the port via <strong>{gateEvent.GateId}</strong> at {gateEvent.Timestamp:G}.</p>
                <p>Any applicable tariffs have been automatically invoiced.</p>
                <br>
                <p>Thank you,<br>PortStream Operations</p>
            ";

            // Send via SMTP Service
            await _emailSender.SendEmailAsync(
                toEmail: $"logistics-{gateEvent.TruckLicensePlate.ToLower()}@example.com", 
                subject: $"Gate Exit Receipt - {gateEvent.TruckLicensePlate}", 
                htmlBody: emailHtml
            );
        }
    }
}
