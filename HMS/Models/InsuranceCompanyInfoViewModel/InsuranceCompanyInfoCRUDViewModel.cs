using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.InsuranceCompanyInfoViewModel
{
    public class InsuranceCompanyInfoCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [Display(Name = "Coverage Details")]
        public string CoverageDetails { get; set; }


        public static implicit operator InsuranceCompanyInfoCRUDViewModel(InsuranceCompanyInfo _InsuranceCompanyInfo)
        {
            return new InsuranceCompanyInfoCRUDViewModel
            {
                Id = _InsuranceCompanyInfo.Id,
                Name = _InsuranceCompanyInfo.Name,
                Address = _InsuranceCompanyInfo.Address,
                Phone = _InsuranceCompanyInfo.Phone,
                Email = _InsuranceCompanyInfo.Email,
                CoverageDetails = _InsuranceCompanyInfo.CoverageDetails,
                CreatedDate = _InsuranceCompanyInfo.CreatedDate,
                ModifiedDate = _InsuranceCompanyInfo.ModifiedDate,
                CreatedBy = _InsuranceCompanyInfo.CreatedBy,
                ModifiedBy = _InsuranceCompanyInfo.ModifiedBy,
                Cancelled = _InsuranceCompanyInfo.Cancelled,
            };
        }

        public static implicit operator InsuranceCompanyInfo(InsuranceCompanyInfoCRUDViewModel vm)
        {
            return new InsuranceCompanyInfo
            {
                Id = vm.Id,
                Name = vm.Name,
                Address = vm.Address,
                Phone = vm.Phone,
                Email = vm.Email,
                CoverageDetails = vm.CoverageDetails,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
