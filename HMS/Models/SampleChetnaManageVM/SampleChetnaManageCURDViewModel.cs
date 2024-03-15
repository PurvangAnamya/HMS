using HMS.Models.ManageUserRolesVM;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.SampleChetnaManageVM
{
    public class SampleChetnaManageCURDViewModel : EntityBase
    {
        [Display(Name = "SL"), Required]
        public Int64 Id { get; set; }
        [Display(Name = "Name"), Required]
        public string Title { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePictureDetails { get; set; }

        [Display(Name = "Profile Picture")]
        public string ProfilePicture { get; set; } = "images/UserIcon/Admin.png";
        public Int64 ImageId { get; set; }
        public string? Description { get; set; }

        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }
        public string ApplicationUserId { get; set; }
        public List<UserImages> listUserImages { get; set; }
        public List<SampleChetnaManageRoleDetails> listSampleChetnaManageRoleDetails { get; set; }     

        public static implicit operator SampleChetnaManageCURDViewModel(SampleChetnaManage vm)
        {
            return new SampleChetnaManageCURDViewModel
            {
                Id = vm.Id,
                Title = vm.Title,
                Description = vm.Description,
                DateOfBirth = vm.DateOfBirth,
                ProfilePicture = vm.ProfilePicture,
                ImageId=vm.ImageId,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
              
        public static implicit operator SampleChetnaManage(SampleChetnaManageCURDViewModel vm)
        {
            return new SampleChetnaManage
            {
                Id = vm.Id,
                Title = vm.Title,
                Description = vm.Description,
                DateOfBirth = vm.DateOfBirth,
                ProfilePicture = vm.ProfilePicture,
                ImageId = vm.ImageId,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
