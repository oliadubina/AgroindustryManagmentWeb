using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentityServer
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // For the lab, we just log the email to the console instead of sending it.
            _logger.LogInformation("-------------------------------------------------");
            _logger.LogInformation($"Sending Email to: {email}");
            _logger.LogInformation($"Subject: {subject}");
            _logger.LogInformation($"Message: {htmlMessage}");
            _logger.LogInformation("-------------------------------------------------");

            return Task.CompletedTask;
        }
    }
}
