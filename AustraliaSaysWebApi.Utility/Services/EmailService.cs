using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EcomWeb.Utility.Services
{

    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = smtpSettings["Host"];
                    smtpClient.Port = int.Parse(smtpSettings["Port"]);
                    smtpClient.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);

                    var message = new MailMessage
                    {
                        From = new MailAddress(smtpSettings["Username"]),
                        Subject = subject,
                        Body = body,
                       

                    };
                    message.IsBodyHtml = true; // This enables HTML body content.

                    message.To.Add(toEmail);

                    // Add logo if provided and is HTML
             

                    await smtpClient.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }

    }

}