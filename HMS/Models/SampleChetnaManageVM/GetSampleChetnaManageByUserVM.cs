using Microsoft.AspNetCore.Identity;

namespace HMS.Models.SampleChetnaManageVM
{
    public class GetSampleChetnaManageByUserVM
    {
        public string ApplicationUserId { get; set; }
        public UserManager<ApplicationUser> UserManager { get; set; }
        public List<IdentityRole> listIdentityRole { get; set; }
    }
}
