using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using server.Interfaces.Services;

namespace server.Services
{
    public class MailerService : IMailerService
    {
        private readonly IConfiguration _configuration;

        public MailerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task SendEmail(string emailTo, string subject, string body)
        {
            var smtpClient = new SmtpClient()
            {
                Host = _configuration["Smtp:Host"]!,
                Port = int.Parse(_configuration["Smtp:Port"]!),
                Credentials = new NetworkCredential(
                    _configuration["Smtp:Username"],
                    _configuration["Smtp:Password"]
                ),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:Username"]!),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(emailTo);
            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendResetPasswordEmail(
            string emailTo,
            string fullname,
            string resetPasswordUrl
        )
        {
            await SendEmail(emailTo, "StayMateHotel - Đặt lại mật khẩu", fullname);
        }
    }
}
