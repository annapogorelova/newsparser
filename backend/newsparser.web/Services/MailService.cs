using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace NewsParser.Services 
{
    /// <summary>
    /// Mailer implementation
    /// </summary>
    public class MailService: IMailService
    {
        private readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
        }

        public Task SendAccountConfirmationEmail(string email, string confirmationToken)
        {
            string websiteUrl = _config["WebsiteUrl"];
            string confirmationLink = $"{websiteUrl}/account-confirmation?confirmationToken={confirmationToken}&email={email}";
            string mailContent = $@"Congratulations! You have created an account on NewsParser.
                Please confirm your account by following <a href='{confirmationLink}'>this link</a>.";
            
            return SendEmail(email, "Account confirmation", mailContent);
        }

        public async Task SendEmail(string email, string subject, string message)
        {
            var mailerConfig = _config.GetSection("Mailer");
            var emailMessage = CreateHtmlMessage(email, subject, message);
        
            using (var client = new SmtpClient())
            {
                var smtpConfig = mailerConfig.GetSection("SMTP");
                await client.ConnectAsync(smtpConfig["Server"], 
                    int.Parse(smtpConfig["Port"]), SecureSocketOptions.None).ConfigureAwait(false);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        public Task SendPasswordResetEmail(string email, string passwordResetToken)
        {
            string websiteUrl = _config["WebsiteUrl"];
            string resetPasswordLink = $"{websiteUrl}/password-reset?passwordResetToken={passwordResetToken}&email={email}";
            string mailContent = $@"You have requested a password reset on NewsParser.
                Please set a new password by following <a href='{resetPasswordLink}'>this link</a>.";
            
            return SendEmail(email, "Password reset", mailContent);
        }

        private MimeMessage CreateHtmlMessage(string email, string subject, string htmlContent)
        {
            var mailerConfig = _config.GetSection("Mailer");

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(mailerConfig["SenderName"], mailerConfig["SenderEmail"]));
            emailMessage.To.Add(new MailboxAddress(string.Empty, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = htmlContent };

            return emailMessage;
        }
    }
}