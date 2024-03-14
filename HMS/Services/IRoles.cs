using HMS.Data;
using HMS.Models;
using HMS.Models.CommonViewModel;
using HMS.Models.ManageUserRolesVM;
using HMS.Pages;

namespace HMS.Services
{
    public interface IRoles
    {
        Task GenerateRolesFromPageList();
        Task<string> CreateSingleRole(string _RoleName);
        Task AddToRoles(ApplicationUser _ApplicationUser);
        Task<MainMenuViewModel> RolebaseMenuLoad(ApplicationUser _ApplicationUser);
        Task<MainMenuViewModel> ManageUserRolesDetailsByUser(ApplicationUser _ApplicationUser, ApplicationDbContext _context);
        Task<List<ManageUserRolesDetails>> GetRolesByUser(GetRolesByUserViewModel vm);
        Task<List<ManageUserRolesDetails>> GetRoleList();
        Task<JsonResultViewModel> UpdateUserRoles(ManageUserRolesCRUDViewModel vm);
    }
}
