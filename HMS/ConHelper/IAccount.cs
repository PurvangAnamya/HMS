using HMS.Models;
using HMS.Models.AccountViewModels;
using HMS.Models.UserProfileViewModel;
using Microsoft.AspNetCore.Identity;

namespace HMS.ConHelper
{
    public interface IAccount
    {
        Task<Tuple<ApplicationUser, IdentityResult>> CreateUserAccount(CreateUserAccountViewModel _CreateUserAccountViewModel);
        Task<Tuple<ApplicationUser, string>> CreateUserProfile(UserProfileCRUDViewModel vm, string LoginUser);     
    }
}
