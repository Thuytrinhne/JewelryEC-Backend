namespace JewelryEC_Backend.Service.IService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

    }
}
