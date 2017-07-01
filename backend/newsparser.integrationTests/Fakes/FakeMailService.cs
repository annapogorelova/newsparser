using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsParser.Services;

namespace NewsParser.IntegrationTests.Fakes
{
    public class FakeMailService : IMailService
    {
        public class Mail
        {
            public string Email { get; set; }
            public string Subject { get; set; }
            public string Message { get; set; }
        }

        public List<Mail> Emails { get; } = new List<Mail>();

        public Task SendAccountConfirmationEmail(string email, string confirmationToken)
        {
            return SendEmail(email, "Account Confirmation", confirmationToken);
        }

        public Task SendEmail(string email, string subject, string message)
        {
            return Task.Factory.StartNew(() => Emails.Add(new Mail
                {
                    Email = email,
                    Message = message,
                    Subject = subject
                })
            );
        }

        public Task SendPasswordResetEmail(string email, string passwordResetToken)
        {
            return SendEmail(email, "Password Recovery", passwordResetToken);
        }
    }
}