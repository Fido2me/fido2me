using SendGrid;
using SendGrid.Helpers.Mail;

namespace Fido2me.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string email, int code);
    }

    public class EmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly ILogger<IEmailService> _logger;
        public EmailService(ISendGridClient sendGridClient, ILogger<IEmailService> logger)
        {
            _sendGridClient = sendGridClient;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string email, int code)
        {
            var subject = "Fido2Me email verification";
            var message = GenerateEmailMessage(email, subject, code);

            var r = await _sendGridClient.SendEmailAsync(message);

            // https://app.sendgrid.com/email_activity for troubleshooting
            return r.IsSuccessStatusCode;
        }

        private SendGridMessage GenerateEmailMessage(string toEmail, string subject, int code)
        {
            var fromEmail = "notifications@fido2me.com";
            var fromName = "Fido2Me";
            var body = $"Your email verification code: {code}";

            var message = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                PlainTextContent = body
            };

            message.AddTo(new EmailAddress(toEmail));

            return message;
        }
    }
}
