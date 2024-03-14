using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.CheckupSummaryViewModel
{
    public class CheckupSummaryCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Patient")]
        public Int64 PatientId { get; set; }
        public Int64 SerialNo { get; set; }
        public string VisitId { get; set; }
        public Int64 PaymentId { get; set; }
        public string PatientName { get; set; }
        [Display(Name = "Doctor")]
        [Required]
        public Int64 DoctorId { get; set; }
        public string DoctorName { get; set; }
        [Required]
        public string PatientType { get; set; }
        [Required]
        public string Symptoms { get; set; }
        [Required]
        public string Diagnosis { get; set; }
        public string HPI { get; set; }
        [Display(Name = "Vital Signs")]
        public string VitalSigns { get; set; }
        [Display(Name = "Physical Examination")]
        public string PhysicalExamination { get; set; }
        public string Comments { get; set; }
        [Display(Name = "Checkup Date")]
        [Required]
        public DateTime CheckupDate { get; set; } = DateTime.Now;
        [Display(Name = "Next Visit")]
        public DateTime NextVisitDate { get; set; } = DateTime.Now.AddDays(7);
        public string Advice { get; set; }
        public int? BPSystolic { get; set; }
        public int? BPDiastolic { get; set; }
        public int? RespirationRate { get; set; }
        public int? Temperature { get; set; }
        public string NursingNotes { get; set; }
        public string CurrentURL { get; set; }


        public static implicit operator CheckupSummaryCRUDViewModel(CheckupSummary _CheckupSummary)
        {
            return new CheckupSummaryCRUDViewModel
            {
                Id = _CheckupSummary.Id,
                PatientId = _CheckupSummary.PatientId,
                SerialNo = _CheckupSummary.SerialNo,
                VisitId = _CheckupSummary.VisitId,
                PaymentId = _CheckupSummary.PaymentId,
                DoctorId = _CheckupSummary.DoctorId,
                PatientType = _CheckupSummary.PatientType,
                Symptoms = _CheckupSummary.Symptoms,
                Diagnosis = _CheckupSummary.Diagnosis,
                HPI = _CheckupSummary.HPI,
                VitalSigns = _CheckupSummary.VitalSigns,
                PhysicalExamination = _CheckupSummary.PhysicalExamination,
                Comments = _CheckupSummary.Comments,
                CheckupDate = _CheckupSummary.CheckupDate,
                NextVisitDate = _CheckupSummary.NextVisitDate,
                Advice = _CheckupSummary.Advice,
                BPSystolic = _CheckupSummary.BPSystolic,
                BPDiastolic = _CheckupSummary.BPDiastolic,
                RespirationRate = _CheckupSummary.RespirationRate,
                Temperature = _CheckupSummary.Temperature,
                NursingNotes = _CheckupSummary.NursingNotes,


                CreatedDate = _CheckupSummary.CreatedDate,
                ModifiedDate = _CheckupSummary.ModifiedDate,
                CreatedBy = _CheckupSummary.CreatedBy,
                ModifiedBy = _CheckupSummary.ModifiedBy,
                Cancelled = _CheckupSummary.Cancelled,
            };
        }

        public static implicit operator CheckupSummary(CheckupSummaryCRUDViewModel vm)
        {
            return new CheckupSummary
            {
                Id = vm.Id,
                PatientId = vm.PatientId,
                SerialNo = vm.SerialNo,
                VisitId = vm.VisitId,
                PaymentId = vm.PaymentId,
                DoctorId = vm.DoctorId,
                PatientType = vm.PatientType,
                Symptoms = vm.Symptoms,
                Diagnosis = vm.Diagnosis,
                HPI = vm.HPI,
                VitalSigns = vm.VitalSigns,
                PhysicalExamination = vm.PhysicalExamination,
                Comments = vm.Comments,
                CheckupDate = vm.CheckupDate,
                NextVisitDate = vm.NextVisitDate,
                Advice = vm.Advice,
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
    }
}
