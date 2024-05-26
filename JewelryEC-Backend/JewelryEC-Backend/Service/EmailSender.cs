using JewelryEC_Backend.Service.IService;
using System.Net.Mail;
using System.Net;

namespace JewelryEC_Backend.Service
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            { 
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mongthitrinhtkp@gmail.com", "hfva wqzf dzmv xzcu")
            };

            await client.SendMailAsync(
                new MailMessage(from: "mongthitrinhtkp@gmail.com",
                                to: email,
                                subject: subject,
                                body: message
                                ));
        }

    }
}
