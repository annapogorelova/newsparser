using System;
using Microsoft.Extensions.Configuration;

namespace NewsParser.Web.Configuration
{
    public static class EnvConfigurationProvider
    {
        public static string AuthSecretKey = Environment.GetEnvironmentVariable("AUTH_SECRET_KEY");
        public static string FacebookAppId = Environment.GetEnvironmentVariable("FACEBOOK_APP_ID");
        public static string GoogleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
        
        public static string MailerHost = Environment.GetEnvironmentVariable("MAILER_SMTP_HOST");
        public static string MailerDomain = Environment.GetEnvironmentVariable("MAILER_SMTP_DOMAIN");
        public static string MailerPort = Environment.GetEnvironmentVariable("MAILER_SMTP_PORT");
        
        public static string MailerUsername = Environment.GetEnvironmentVariable("MAILER_SMTP_USERNAME");
        public static string MailerPassword = Environment.GetEnvironmentVariable("MAILER_SMTP_PASSWORD");

        public static string MailerSenderName = Environment.GetEnvironmentVariable("MAILER_SENDER_NAME");
        public static string MailerSenderEmail = Environment.GetEnvironmentVariable("MAILER_SENDER_EMAIL");
        
        public static string FrontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");

        public static string GetDbConnectionString()
        {
            string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            string dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            string dbName = Environment.GetEnvironmentVariable("DB_NAME");
            string dbUser = Environment.GetEnvironmentVariable("DB_USER");
            string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            return $"server={dbHost};userid={dbUser};pwd={dbPassword};port={dbPort};database={dbName};sslmode=none;charset=utf8;";
        }
    }   
}