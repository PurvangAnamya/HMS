using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.CheckupMedicineDetailsViewModel
{
    public class CheckupMedicineDetailsCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public Int64 CheckupMedicineDetailsId { get; set; }
        public Int64 PaymentId { get; set; }
        public Int64 CheckupId { get; set; }
        public string VisitId { get; set; }
        public Int64 MedicineId { get; set; }
        public string MedicineName { get; set; }
        public int NoofDays { get; set; }
        public string WhentoTake { get; set; }
        public int WhentoTakeDayCount { get; set; }
        public bool IsBeforeMeal { get; set; }



        public static implicit operator CheckupMedicineDetailsCRUDViewModel(CheckupMedicineDetails _CheckupMedicineDetails)
        {
            return new CheckupMedicineDetailsCRUDViewModel
            {
                Id = _CheckupMedicineDetails.Id,
                VisitId = _CheckupMedicineDetails.VisitId,
                MedicineId = _CheckupMedicineDetails.MedicineId,
                NoofDays = _CheckupMedicineDetails.NoofDays,
                WhentoTake = _CheckupMedicineDetails.WhentoTake,
                IsBeforeMeal = _CheckupMedicineDetails.IsBeforeMeal,
                CreatedDate = _CheckupMedicineDetails.CreatedDate,
                ModifiedDate = _CheckupMedicineDetails.ModifiedDate,
                CreatedBy = _CheckupMedicineDetails.CreatedBy,
                ModifiedBy = _CheckupMedicineDetails.ModifiedBy,
                Cancelled = _CheckupMedicineDetails.Cancelled,

            };
        }

        public static implicit operator CheckupMedicineDetails(CheckupMedicineDetailsCRUDViewModel vm)
        {
            return new CheckupMedicineDetails
            {
                Id = vm.Id,
                VisitId = vm.VisitId,
                MedicineId = vm.MedicineId,
                NoofDays = vm.NoofDays,
                WhentoTake = vm.WhentoTake,
                IsBeforeMeal = vm.IsBeforeMeal,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,

            };
        }
    }
}

