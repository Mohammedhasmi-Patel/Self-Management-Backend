

namespace SelfManagement.Application.ServiceInterface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail,string toSubject,string htmlMessage);
    }
}
