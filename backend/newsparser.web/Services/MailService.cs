using System.Net;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using NewsParser.Web.Configuration;

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
            string websiteUrl = EnvConfigurationProvider.FrontendUrl;
            string confirmationLink = $"{websiteUrl}/email-confirmation?confirmationToken={confirmationToken}&email={email}";
            string mailContent = $@"You have recently created the account on FeedsCatcher.<br/>
            If it was not you, please ignore this letter.<br/>
            If it was you, please confirm your registration by following <a href='{confirmationLink}'>this link</a>.<br/>";

            return SendEmail(email, "Account confirmation", mailContent);
        }

        public Task SendEmailConfirmationEmail(string email, string confirmationToken)
        {
            string websiteUrl = EnvConfigurationProvider.FrontendUrl;
            string confirmationLink = $"{websiteUrl}/email-confirmation?confirmationToken={confirmationToken}&email={email}";
            string mailContent = $@"You have recently changed your email on <a href='{EnvConfigurationProvider.FrontendUrl}'>FeedsCatcher</a>.<br/>
            Please confirm your new email by following <a href='{confirmationLink}'>this link</a>.";

            return SendEmail(email, "Email confirmation", mailContent);
        }

        public async Task SendEmail(string email, string subject, string message)
        {
            var emailMessage = CreateHtmlMessage(email, subject, message);
        
            using (var client = new SmtpClient())
            {              
                await client.ConnectAsync(
                    EnvConfigurationProvider.MailerHost, 
                    int.Parse(EnvConfigurationProvider.MailerPort));
                
                await client.AuthenticateAsync(
                    new NetworkCredential(
                        EnvConfigurationProvider.MailerUsername,
                        EnvConfigurationProvider.MailerPassword
                    )
                );
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        public Task SendPasswordResetEmail(string email, string passwordResetToken)
        {
            string websiteUrl = EnvConfigurationProvider.FrontendUrl;
            string resetPasswordLink = $"{websiteUrl}/password-reset?passwordResetToken={passwordResetToken}&email={email}";
            string mailContent = $@"You have recently requested a password reset on <a href='{EnvConfigurationProvider.FrontendUrl}'>FeedsCatcher</a>.<br/>
                Please set a new password by following <a href='{resetPasswordLink}'>this link</a>.";
            
            return SendEmail(email, "Password reset", mailContent);
        }

        private MimeMessage CreateHtmlMessage(string email, string subject, string htmlContent)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(
                EnvConfigurationProvider.MailerSenderName, 
                EnvConfigurationProvider.MailerSenderEmail
            ));
            emailMessage.To.Add(new MailboxAddress(string.Empty, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = htmlContent };

            return emailMessage;
        }
    }
}