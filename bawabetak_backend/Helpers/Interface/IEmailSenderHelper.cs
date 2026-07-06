namespace bawabetak_backend.Helpers.Interface
{
    public interface IEmailSenderHelper
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
