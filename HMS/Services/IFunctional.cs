using HMS.Models;
using HMS.Models.DashboardViewModel;
using System.Security.Claims;

namespace HMS.Services
{
    public interface IFunctional
    {
        Task InitAppData();
        Task GenerateUserUserRole();
        Task CreateDefaultSuperAdmin();
        Task CreateDefaultHospital();

        Task SendEmailBySendGridAsync(string apiKey,
            string fromEmail,
            string fromFullName,
            string subject,
            string message,
            string email);

        Task SendEmailByGmailAsync(string fromEmail,
            string fromFullName,
            string subject,
            string messageBody,
            string toEmail,
            string toFullName,
            string smtpUser,
            string smtpPassword,
            string smtpHost,
            int smtpPort,
            bool smtpSSL);

        Task CreateDefaultEmailSettings();
        Task<SharedUIDataViewModel> GetSharedUIData(ClaimsPrincipal _ClaimsPrincipal);

        Task CreateDefaultDoctorUser();
        Task CreateDefaultOtherUser();
        Task CreateDefaultHospitalUser();
        Task CreateDefaultPatientUser();

        Task CreateDefaultIdentitySettings();
        Task<DefaultIdentityOptions> GetDefaultIdentitySettings();

        Task<string> UploadFile(List<IFormFile> files, IWebHostEnvironment env, string uploadFolder);

    }
}
