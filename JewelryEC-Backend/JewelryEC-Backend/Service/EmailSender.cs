using JewelryEC_Backend.Service.IService;
using System.Net.Mail;
using System.Net;
using JewelryEC_Backend.Utility;

namespace JewelryEC_Backend.Service
{
    public class EmailSender (IConfiguration _config): IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient(_config[MailConfigKey.Mail_Host], int.Parse(_config[MailConfigKey.Mail_Port]!))
            { 
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_config[MailConfigKey.Mail_Email], _config[MailConfigKey.Mail_Password])
            };

            await client.SendMailAsync(
                new MailMessage(from: _config[MailConfigKey.Mail_Email]!,
                                to: email,
                                subject: subject,
                                body: message
                                ));
        }

    }
}
