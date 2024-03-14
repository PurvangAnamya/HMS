using System.ComponentModel.DataAnnotations;
using HMS.Models.UserProfileViewModel;

namespace HMS.Models.DoctorsInfoViewModel
{
    public class DoctorsInfoCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public string ApplicationUserId { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Display(Name = "Designation")]
        public Int64 DesignationId { get; set; }
        public string DesignationDisplay { get; set; }
        [Display(Name = "Doctors ID")]
        public string DoctorsID { get; set; }
        [Display(Name = "Doctor Fee")]
        public double? DoctorFee { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public int UserType { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("PasswordHash", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string OldPassword { get; set; }
        public Int64 RoleId { get; set; }
        public string RoleIdDisplay { get; set; }
        [Display(Name = "Profile Picture")]
        public string ProfilePicture { get; set; } = "/upload/blank-person.png";
        public IFormFile ProfilePictureDetails { get; set; }

        public static implicit operator DoctorsInfoCRUDViewModel(UserProfileCRUDViewModel vm)
        {
            return new DoctorsInfoCRUDViewModel
            {
                Id = vm.UserProfileId,
                ApplicationUserId = vm.ApplicationUserId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                PhoneNumber = vm.PhoneNumber,
                Email = vm.Email,
                Address = vm.Address,
                Country = vm.Country,
                UserType = vm.UserType,
                PasswordHash = vm.PasswordHash,
                ConfirmPassword = vm.ConfirmPassword,
                RoleId = vm.RoleId,
                ProfilePicture = vm.ProfilePicture,
                ProfilePictureDetails = vm.ProfilePictureDetails,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled
            };
        }

        public static implicit operator UserProfileCRUDViewModel(DoctorsInfoCRUDViewModel vm)
        {
            return new UserProfileCRUDViewModel
            {
                UserProfileId = vm.Id,
                ApplicationUserId = vm.ApplicationUserId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                PhoneNumber = vm.PhoneNumber,
                Email = vm.Email,
                Address = vm.Address,
                Country = vm.Country,
                UserType = vm.UserType,
                PasswordHash = vm.PasswordHash,
                ConfirmPassword = vm.ConfirmPassword,
                RoleId = vm.RoleId,
                ProfilePicture = vm.ProfilePicture,
                ProfilePictureDetails = vm.ProfilePictureDetails,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
