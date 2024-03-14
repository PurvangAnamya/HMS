using HMS.Models.CheckupSummaryViewModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.VitalSignsViewModel
{
    public class VitalSignsCRUDViewModel : EntityBase
    {
        public Int64 CheckupSummaryId { get; set; }
        [Display(Name = "BP Systolic")]
        public int? BPSystolic { get; set; }
        [Display(Name = "BP Diastolic")]
        public int? BPDiastolic { get; set; }
        [Display(Name = "Respiration Rate")]
        public int? RespirationRate { get; set; }
        [Display(Name = "Temperature(°F)")]
        public int? Temperature { get; set; }
        [Display(Name = "Nursing Notes")]
        public string NursingNotes { get; set; }



        public static implicit operator VitalSignsCRUDViewModel(CheckupSummary _VitalSigns)
        {
            return new VitalSignsCRUDViewModel
            {
                CheckupSummaryId = _VitalSigns.Id,
                BPSystolic = _VitalSigns.BPSystolic,
                BPDiastolic = _VitalSigns.BPDiastolic,
                RespirationRate = _VitalSigns.RespirationRate,
                Temperature = _VitalSigns.Temperature,
                NursingNotes = _VitalSigns.NursingNotes,
                CreatedDate = _VitalSigns.CreatedDate,
                ModifiedDate = _VitalSigns.ModifiedDate,
                CreatedBy = _VitalSigns.CreatedBy,
                ModifiedBy = _VitalSigns.ModifiedBy,
                Cancelled = _VitalSigns.Cancelled,

            };
        }

        public static implicit operator CheckupSummary(VitalSignsCRUDViewModel vm)
        {
            return new CheckupSummary
            {
                Id = vm.CheckupSummaryId,
                BPSystolic = vm.BPSystolic,
                BPDiastolic = vm.BPDiastolic,
                RespirationRate = vm.RespirationRate,
                Temperature = vm.Temperature,
                NursingNotes = vm.NursingNotes,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }

        public static implicit operator VitalSignsCRUDViewModel(CheckupSummaryCRUDViewModel _VitalSigns)
        {
            return new VitalSignsCRUDViewModel
            {
                CheckupSummaryId = _VitalSigns.Id,
                BPSystolic = _VitalSigns.BPSystolic,
                BPDiastolic = _VitalSigns.BPDiastolic,
                RespirationRate = _VitalSigns.RespirationRate,
                Temperature = _VitalSigns.Temperature,
                NursingNotes = _VitalSigns.NursingNotes,
                CreatedDate = _VitalSigns.CreatedDate,
                ModifiedDate = _VitalSigns.ModifiedDate,
                CreatedBy = _VitalSigns.CreatedBy,
                ModifiedBy = _VitalSigns.ModifiedBy,
                Cancelled = _VitalSigns.Cancelled,
            };
        }
    }
}
