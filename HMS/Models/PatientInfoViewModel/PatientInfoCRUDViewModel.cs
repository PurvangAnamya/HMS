using System.ComponentModel.DataAnnotations;
using HMS.Models.UserProfileViewModel;

namespace HMS.Models.PatientInfoViewModel
{
    public class PatientInfoCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string PatientCode { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string FullName { get; set; }
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Spouse Name")]
        public string SpouseName { get; set; }
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }
        [Display(Name = "Mother Name")]
        public string MotherName { get; set; }
        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; } = DateTime.Today;
        [Display(Name = "Registration Fee")]
        public double? RegistrationFee { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public bool? Agreement { get; set; }
        public string Remarks { get; set; }
        public string ProfilePicture { get; set; } = "/upload/blank-person.png";
        public IFormFile ProfilePictureDetails { get; set; }
        public int UserType { get; set; }
        public Int64 RoleId { get; set; }

        [Display(Name = "Password")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        [Display(Name = "Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("PasswordHash", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public static implicit operator PatientInfoCRUDViewModel(PatientInfo _PatientInfo)
        {
            return new PatientInfoCRUDViewModel
            {
                Id = _PatientInfo.Id,
                ApplicationUserId = _PatientInfo.ApplicationUserId,
                PatientCode = _PatientInfo.PatientCode,
                FirstName = _PatientInfo.FirstName,
                LastName = _PatientInfo.LastName,
                MaritalStatus = _PatientInfo.MaritalStatus,
                Gender = _PatientInfo.Gender,
                SpouseName = _PatientInfo.SpouseName,
                BloodGroup = _PatientInfo.BloodGroup,
                FatherName = _PatientInfo.FatherName,
                MotherName = _PatientInfo.MotherName,
                DateOfBirth = _PatientInfo.DateOfBirth,
                RegistrationFee = _PatientInfo.RegistrationFee,
                Phone = _PatientInfo.Phone,
                Email = _PatientInfo.Email,
                Address = _PatientInfo.Address,
                Country = _PatientInfo.Country,
                Agreement = _PatientInfo.Agreement,
                Remarks = _PatientInfo.Remarks,
                ProfilePicture = _PatientInfo.ProfilePicture,
                CreatedDate = _PatientInfo.CreatedDate,
                ModifiedDate = _PatientInfo.ModifiedDate,
                CreatedBy = _PatientInfo.CreatedBy,
                ModifiedBy = _PatientInfo.ModifiedBy,
                Cancelled = _PatientInfo.Cancelled,
            };
        }

        public static implicit operator PatientInfo(PatientInfoCRUDViewModel vm)
        {
            return new PatientInfo
            {
                Id = vm.Id,
                ApplicationUserId = vm.ApplicationUserId,
                PatientCode = vm.PatientCode,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                MaritalStatus = vm.MaritalStatus,
                Gender = vm.Gender,
                SpouseName = vm.SpouseName,
                BloodGroup = vm.BloodGroup,
                FatherName = vm.FatherName,
                MotherName = vm.MotherName,
                DateOfBirth = vm.DateOfBirth,
                RegistrationFee = vm.RegistrationFee,
                Phone = vm.Phone,
                Email = vm.Email,
                Address = vm.Address,
                Country = vm.Country,
                Agreement = vm.Agreement,
                Remarks = vm.Remarks,
                ProfilePicture = vm.ProfilePicture,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }

        public static implicit operator UserProfileCRUDViewModel(PatientInfoCRUDViewModel vm)
        {
            return new UserProfileCRUDViewModel
            {
                ApplicationUserId = vm.ApplicationUserId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                DateOfBirth = (DateTime)vm.DateOfBirth,
                PhoneNumber = vm.Phone,
                Email = vm.Email,
                Address = vm.Address,
                Country = vm.Country,
                ProfilePicture = vm.ProfilePicture,
                PasswordHash = vm.PasswordHash,
                ConfirmPassword = vm.ConfirmPassword,
                UserType = vm.UserType,
                RoleId = vm.RoleId,
                
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
