using System.Threading.Tasks;

namespace HMS.Services
{
    public interface IEmailSender
    {
        Task<Task> SendEmailAsync(string email, string subject, string message);
    }
}
