using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.PatientTestViewModel
{
    public class PatientTestCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Required]
        [Display(Name = "Patient")]
        public Int64 PatientId { get; set; }
        public string PatientName { get; set; }
        public string VisitId { get; set; }
        public string ApplicationUserId { get; set; }
        [Required]
        [Display(Name = "Test Date")]
        public DateTime TestDate { get; set; } = DateTime.Now;
        [Required]
        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; } = DateTime.Now.AddDays(1);
        [Required]
        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; }


        public static implicit operator PatientTestCRUDViewModel(PatientTest _PatientTest)
        {
            return new PatientTestCRUDViewModel
            {
                Id = _PatientTest.Id,
                PatientId = _PatientTest.PatientId,
                VisitId = _PatientTest.VisitId,
                ApplicationUserId = _PatientTest.ApplicationUserId,
                TestDate = _PatientTest.TestDate,
                DeliveryDate = _PatientTest.DeliveryDate,
                PaymentStatus = _PatientTest.PaymentStatus,
                CreatedDate = _PatientTest.CreatedDate,
                ModifiedDate = _PatientTest.ModifiedDate,
                CreatedBy = _PatientTest.CreatedBy,
                ModifiedBy = _PatientTest.ModifiedBy,
                Cancelled = _PatientTest.Cancelled,

            };
        }

        public static implicit operator PatientTest(PatientTestCRUDViewModel vm)
        {
            return new PatientTest
            {
                Id = vm.Id,
                PatientId = vm.PatientId,
                VisitId = vm.VisitId,
                ApplicationUserId = vm.ApplicationUserId,
                TestDate = vm.TestDate,
                DeliveryDate = vm.DeliveryDate,
                PaymentStatus = vm.PaymentStatus,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
