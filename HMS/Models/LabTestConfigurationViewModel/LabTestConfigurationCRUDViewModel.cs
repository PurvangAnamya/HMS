using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.LabTestConfigurationViewModel
{
    public class LabTestConfigurationCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public Int64 LabTestsId { get; set; }
        [Display(Name = "Lab Test Name")]
        public string LabTestName { get; set; }
        [Display(Name = "Lab Test Category Name")]
        public string LabTestCategoryName { get; set; }
        public int? Sorting { get; set; }
        [Display(Name = "Report Group")]
        public string ReportGroup { get; set; }
        [Display(Name = "Name Of Test")]
        [Required]
        public string NameOfTest { get; set; }
        [Display(Name = "Result")]
        [Required]
        public string Result { get; set; }
        [Display(Name = "Normal Value")]
        [Required]
        public string NormalValue { get; set; }


        public static implicit operator LabTestConfigurationCRUDViewModel(LabTestConfiguration _LabTestConfiguration)
        {
            return new LabTestConfigurationCRUDViewModel
            {
                Id = _LabTestConfiguration.Id,
                LabTestsId = _LabTestConfiguration.LabTestsId,
                Sorting = _LabTestConfiguration.Sorting,
                ReportGroup = _LabTestConfiguration.ReportGroup,
                NameOfTest = _LabTestConfiguration.NameOfTest,
                Result = _LabTestConfiguration.Result,
                NormalValue = _LabTestConfiguration.NormalValue,
                CreatedDate = _LabTestConfiguration.CreatedDate,
                ModifiedDate = _LabTestConfiguration.ModifiedDate,
                CreatedBy = _LabTestConfiguration.CreatedBy,
                ModifiedBy = _LabTestConfiguration.ModifiedBy,
                Cancelled = _LabTestConfiguration.Cancelled,
            };
        }

        public static implicit operator LabTestConfiguration(LabTestConfigurationCRUDViewModel vm)
        {
            return new LabTestConfiguration
            {
                Id = vm.Id,
                LabTestsId = vm.LabTestsId,
                Sorting = vm.Sorting,
                ReportGroup = vm.ReportGroup,
                NameOfTest = vm.NameOfTest,
                Result = vm.Result,
                NormalValue = vm.NormalValue,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
