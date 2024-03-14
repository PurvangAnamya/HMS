using System.ComponentModel.DataAnnotations;

namespace HMS.Models.PaymentCategoriesViewModel
{
    public class PaymentCategoriesCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public int Id { get; set; }
        public string PaymentItemCode { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Unit Price")]
        [Required]
        public double UnitPrice { get; set; }
        [Required]
        public string Description { get; set; }



        public static implicit operator PaymentCategoriesCRUDViewModel(PaymentCategories _PaymentCategories)
        {
            return new PaymentCategoriesCRUDViewModel
            {
                Id = _PaymentCategories.Id,
                PaymentItemCode = _PaymentCategories.PaymentItemCode,
                Name = _PaymentCategories.Name,
                UnitPrice = _PaymentCategories.UnitPrice,
                Description = _PaymentCategories.Description,
                CreatedDate = _PaymentCategories.CreatedDate,
                ModifiedDate = _PaymentCategories.ModifiedDate,
                CreatedBy = _PaymentCategories.CreatedBy,
                ModifiedBy = _PaymentCategories.ModifiedBy,
                Cancelled = _PaymentCategories.Cancelled,

            };
        }

        public static implicit operator PaymentCategories(PaymentCategoriesCRUDViewModel vm)
        {
            return new PaymentCategories
            {
                Id = vm.Id,
                PaymentItemCode = vm.PaymentItemCode,
                Name = vm.Name,
                UnitPrice = vm.UnitPrice,
                Description = vm.Description,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,

            };
        }
    }
}
