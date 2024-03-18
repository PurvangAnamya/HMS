using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.LabTestsViewModel
{
    public class LabTestsCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Lab Test Category")]
        [Required]
        public Int64 LabTestCategoryId { get; set; }
        public string LabTestCategoryName { get; set; }
        public string PaymentItemCode { get; set; }
        [Display(Name = "Lab Test Name")]
        [Required]
        public string LabTestName { get; set; }
        public string Unit { get; set; }
        [Display(Name = "Unit Price")]
        public double UnitPrice { get; set; }
        [Display(Name = "Reference Range")]
        public string ReferenceRange { get; set; }
        public bool Status { get; set; }
        public string Hospital { get; set; }



        public static implicit operator LabTestsCRUDViewModel(LabTests _LabTests)
        {
            return new LabTestsCRUDViewModel
            {
                Id = _LabTests.Id,
                LabTestCategoryId = _LabTests.LabTestCategoryId,
                PaymentItemCode = _LabTests.PaymentItemCode,
                LabTestName = _LabTests.LabTestName,
                Unit = _LabTests.Unit,
                UnitPrice = _LabTests.UnitPrice,
                ReferenceRange = _LabTests.ReferenceRange,
                Status = _LabTests.Status,
                CreatedDate = _LabTests.CreatedDate,
                ModifiedDate = _LabTests.ModifiedDate,
                CreatedBy = _LabTests.CreatedBy,
                ModifiedBy = _LabTests.ModifiedBy,
                Cancelled = _LabTests.Cancelled,

            };
        }

        public static implicit operator LabTests(LabTestsCRUDViewModel vm)
        {
            return new LabTests
            {
                Id = vm.Id,
                LabTestCategoryId = vm.LabTestCategoryId,
                PaymentItemCode = vm.PaymentItemCode,
                LabTestName = vm.LabTestName,
                Unit = vm.Unit,
                UnitPrice = vm.UnitPrice,
                ReferenceRange = vm.ReferenceRange,
                Status = vm.Status,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }

    }
}