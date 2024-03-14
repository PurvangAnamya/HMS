using HMS.Services;

namespace HMS.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, IFunctional functional)
        {
            context.Database.EnsureCreated();
            if (context.ApplicationUser.Any())
            {
                return;
            }
            else
            {
                // Int64 hospitalId = await functional.CreateDefaultHospital();
                await functional.CreateDefaultHospital();
                await functional.CreateDefaultHospitalUser();
                await functional.CreateDefaultSuperAdmin();
                await functional.CreateDefaultEmailSettings();
                await functional.CreateDefaultIdentitySettings();

                await functional.InitAppData();
                await functional.GenerateUserUserRole();
                await functional.CreateDefaultDoctorUser();
                await functional.CreateDefaultOtherUser();
                await functional.CreateDefaultPatientUser();
            }
        }
    }
}
