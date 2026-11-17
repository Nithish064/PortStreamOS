using Serilog;
using NotificationService.Services;

var builder = Host.CreateApplicationBuilder(args);

// Configure Serilog
builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Logging.AddSerilog();

// Register SMTP and Kafka Services
builder.Services.AddSingleton<SmtpEmailSender>();
builder.Services.AddHostedService<KafkaNotificationConsumer>();

var host = builder.Build();
host.Run();
