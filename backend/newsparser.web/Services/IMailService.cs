using System.Threading.Tasks;

namespace NewsParser.Services 
{
    /// <summary>
    /// Interface contains methods declarations for Mailer functionality
    /// </summary>
    public interface IMailService 
    {
        /// <summary>
        /// Sends the email letter to the specified address
        /// </summary>
        /// <param name="email">Receiver email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="message">Email content</param>
        /// <returns></returns>
        Task SendEmail(string email, string subject, string message);

        /// <summary>
        /// Sends the account confirmation email to the specified address
        /// </summary>
        /// <param name="email">Receiver email address</param>
        /// <param name="confirmationToken">Account confirmation token</param>
        /// <returns></returns>
        Task SendAccountConfirmationEmail(string email, string confirmationToken);
    }
}