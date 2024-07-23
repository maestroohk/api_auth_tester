using System.Net.Mail;
using System.Net;
using tester.Services.Interfaces;

namespace tester.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void SendPasswordResetEmail(string toEmail, string token)
        {
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var smtpHost = _configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var smtpUser = _configuration["EmailSettings:SmtpUser"];
            var smtpPass = _configuration["EmailSettings:SmtpPass"];

            var mailMessage = new MailMessage(fromEmail, toEmail)
            {
                Subject = "Password Reset Request",
                Body = $"Please reset your password using this token: {token}",
                IsBodyHtml = true,
            };

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
        }
    }
}
