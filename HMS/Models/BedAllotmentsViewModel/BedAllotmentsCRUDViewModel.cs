using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.BedAllotmentsViewModel
{
    public class BedAllotmentsCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Patient")]
        [Required]
        public Int64 PatientId { get; set; }
        [Display(Name = "Bed Category")]
        [Required]
        public Int64 BedCategoryId { get; set; }
        [Display(Name = "Bed")]
        [Required]
        public Int64 BedId { get; set; }
        [Display(Name = "Released")]
        public bool IsReleased { get; set; }
        [Display(Name = "Allotment Date")]
        [Required]
        public DateTime AllotmentDate { get; set; } = DateTime.Now;
        [Display(Name = "Discharge Date")]
        [Required]
        public DateTime DischargeDate { get; set; } = DateTime.Now.AddDays(1);
        public string Note { get; set; }


        public static implicit operator BedAllotmentsCRUDViewModel(BedAllotments _BedAllotments)
        {
            return new BedAllotmentsCRUDViewModel
            {
                Id = _BedAllotments.Id,
                PatientId = _BedAllotments.PatientId,
                BedCategoryId = _BedAllotments.BedCategoryId,
                BedId = _BedAllotments.BedId,
                IsReleased = _BedAllotments.IsReleased,
                AllotmentDate = _BedAllotments.AllotmentDate,
                DischargeDate = _BedAllotments.DischargeDate,
                Note = _BedAllotments.Note,
                CreatedDate = _BedAllotments.CreatedDate,
                ModifiedDate = _BedAllotments.ModifiedDate,
                CreatedBy = _BedAllotments.CreatedBy,
                ModifiedBy = _BedAllotments.ModifiedBy,
                Cancelled = _BedAllotments.Cancelled,

            };
        }

        public static implicit operator BedAllotments(BedAllotmentsCRUDViewModel vm)
        {
            return new BedAllotments
            {
                Id = vm.Id,
                BedCategoryId = vm.BedCategoryId,
                PatientId = vm.PatientId,
                BedId = vm.BedId,
                IsReleased = vm.IsReleased,
                AllotmentDate = vm.AllotmentDate,
                DischargeDate = vm.DischargeDate,
                Note = vm.Note,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
