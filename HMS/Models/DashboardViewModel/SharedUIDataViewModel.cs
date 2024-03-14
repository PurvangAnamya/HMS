using HMS.Models.AccountViewModels;
using HMS.Models.CommonViewModel;
using HMS.Pages;

namespace HMS.Models.DashboardViewModel
{
    public class SharedUIDataViewModel
    {
        public UserProfile UserProfile { get; set; }
        public ApplicationInfo ApplicationInfo { get; set; }
        public MainMenuViewModel MainMenuViewModel { get; set; }
        public LoginViewModel LoginViewModel { get; set; }
    }
}
