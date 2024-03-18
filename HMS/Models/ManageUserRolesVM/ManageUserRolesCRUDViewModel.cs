using System.ComponentModel.DataAnnotations;

namespace HMS.Models.ManageUserRolesVM
{
    public class ManageUserRolesCRUDViewModel : EntityBase
    {
        [Display(Name = "SL"), Required]
        public Int64 Id { get; set; }
        [Display(Name = "Name"), Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Int64? ImageId { get; set; }
        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePictureDetails { get; set; }
        public string ProfilePicture { get; set; } = "/upload/blank-person.png";
        public Int64? DashboardImageId { get; set; }
        [Display(Name = "Profile Picture")]
        public IFormFile DashboardPictureDetails { get; set; }
        public string DashboardPicture { get; set; } = "/upload/blank-person.png";
        public string ApplicationUserId { get; set; } 
        public List<ManageUserRolesDetails> listManageUserRolesDetails { get; set; }

        public static implicit operator ManageUserRolesCRUDViewModel(ManageUserRoles vm)
        {
            return new ManageUserRolesCRUDViewModel
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                ImageId = vm.ImageId,
                DashboardImageId = vm.DashboardImageId,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }

        public static implicit operator ManageUserRoles(ManageUserRolesCRUDViewModel vm)
        {
            return new ManageUserRoles
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                ImageId = (long)vm.ImageId,
                DashboardImageId = (long)vm.DashboardImageId,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
