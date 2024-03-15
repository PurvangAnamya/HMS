using System.ComponentModel.DataAnnotations;
namespace HMS.Models.SampleChetnaManageVM
{
    public class SampleChetnaManageViewModel
    {
        public Int64 ManageRoleDetailsId { get; set; }
        public string RoleId { get; set; }
        public Int64 RoleId_SL { get; set; }
        public string RoleName { get; set; }
        public bool IsAllowed { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePictureDetails { get; set; }
        public string ProfilePicture { get; set; } = "/upload/blank-person.png";
        public Int64 ImageId { get; set; }
    }

    public class UpdateSampleChetnaViewModel
    {
        public string ApplicationUserId { get; set; }
        public string CurrentURL { get; set; }
        public List<SampleChetnaManageViewModel> listSampleChetnaManageViewModel { get; set; }
    }

}
