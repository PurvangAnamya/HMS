using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.PatientTestDetailViewModel
{
    public class PatientTestDetailCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public Int64 PatientTestId { get; set; }
        public Int64 LabTestsId { get; set; }
        public string LabTestsName { get; set; }
        public string Result { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string Remarks { get; set; }



        public static implicit operator PatientTestDetailCRUDViewModel(PatientTestDetail _PatientTestDetail)
        {
            return new PatientTestDetailCRUDViewModel
            {
                Id = _PatientTestDetail.Id,
                PatientTestId = _PatientTestDetail.PatientTestId,
                LabTestsId = _PatientTestDetail.LabTestsId,
                Result = _PatientTestDetail.Result,
                Quantity = _PatientTestDetail.Quantity,
                UnitPrice = _PatientTestDetail.UnitPrice,
                Remarks = _PatientTestDetail.Remarks,
                CreatedDate = _PatientTestDetail.CreatedDate,
                ModifiedDate = _PatientTestDetail.ModifiedDate,
                CreatedBy = _PatientTestDetail.CreatedBy,
                ModifiedBy = _PatientTestDetail.ModifiedBy,
                Cancelled = _PatientTestDetail.Cancelled,

            };
        }

        public static implicit operator PatientTestDetail(PatientTestDetailCRUDViewModel vm)
        {
            return new PatientTestDetail
            {
                Id = vm.Id,
                PatientTestId = vm.PatientTestId,
                LabTestsId = vm.LabTestsId,
                Result = vm.Result,
                Quantity = vm.Quantity,
                UnitPrice = vm.UnitPrice,
                Remarks = vm.Remarks,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
