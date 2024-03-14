using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.DoctorsInfoViewModel
{
    public class DoctorsInfoEditViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Application User Id")]
        public string ApplicationUserId { get; set; }
        [Display(Name = "Designation")]
        public Int64 DesignationId { get; set; }
        public string DesignationDisplay { get; set; }
        [Display(Name = "Doctors ID")]
        public string DoctorsID { get; set; }
        [Display(Name = "Doctors Fee")]
        public double? DoctorFee { get; set; }

        public static implicit operator DoctorsInfoEditViewModel(DoctorsInfo _DoctorsInfo)
        {
            return new DoctorsInfoEditViewModel
            {
                Id = _DoctorsInfo.Id,
                ApplicationUserId = _DoctorsInfo.ApplicationUserId,
                DesignationId = _DoctorsInfo.DesignationId,
                DoctorsID = _DoctorsInfo.DoctorsID,
                DoctorFee = _DoctorsInfo.DoctorFee,

                CreatedDate = _DoctorsInfo.CreatedDate,
                ModifiedDate = _DoctorsInfo.ModifiedDate,
                CreatedBy = _DoctorsInfo.CreatedBy,
                ModifiedBy = _DoctorsInfo.ModifiedBy,
            };
        }

        public static implicit operator DoctorsInfo(DoctorsInfoEditViewModel vm)
        {
            return new DoctorsInfo
            {
                Id = vm.Id,
                ApplicationUserId = vm.ApplicationUserId,
                DesignationId = vm.DesignationId,
                DoctorsID = vm.DoctorsID,
                DoctorFee = vm.DoctorFee,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
            };
        }
    }
}
