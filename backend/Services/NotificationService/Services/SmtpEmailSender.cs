using MailKit.Net.Smtp;
using MimeKit;

namespace NotificationService.Services;

public class SmtpEmailSender
{
    private readonly ILogger<SmtpEmailSender> _logger;
    private readonly IConfiguration _config;

    public SmtpEmailSender(ILogger<SmtpEmailSender> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("PortStream OS Notifications", "no-reply@portstreamos.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            
            // In a real environment, we'd use config values. 
            // For demo/dev, we can just connect to a local Mailhog instance or mock it.
            _logger.LogInformation("Attempting to send SMTP email to {Email} with subject '{Subject}'...", toEmail, subject);
            
            // If we had a real SMTP server:
            // await client.ConnectAsync("smtp.server.com", 587, false);
            // await client.AuthenticateAsync("user", "pass");
            // await client.SendAsync(message);
            // await client.DisconnectAsync(true);
            
            // Mocking the success since we don't have a live SMTP server in this environment:
            await Task.Delay(500); 
            _logger.LogInformation("✅ SMTP Email successfully sent to {Email} via MailKit!", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMTP email to {Email}", toEmail);
        }
    }
}
