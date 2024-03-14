using HMS.Data;
using HMS.Models;
using HMS.Services;
using Microsoft.AspNetCore.Identity;

namespace HMS.Helpers
{
    public static class ProgramTaskExtension
    {
        public static ApplicationDbContext GetDBContextInstance(IServiceCollection services)
        {
            var _IServiceScopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
            var _CreateScope = _IServiceScopeFactory.CreateScope();
            var _ServiceProvider = _CreateScope.ServiceProvider;
            var _context = _ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return _context;
        }
        public static void SeedingData(WebApplication app)
        {
            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<ApplicationDbContext>();
                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                        var functional = services.GetRequiredService<IFunctional>();

                        DbInitializer.Initialize(context, functional).Wait();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while seeding the database.");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
