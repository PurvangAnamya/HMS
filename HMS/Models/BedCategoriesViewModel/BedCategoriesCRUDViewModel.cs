using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.BedCategoriesViewModel
{
    public class BedCategoriesCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }



        public static implicit operator BedCategoriesCRUDViewModel(BedCategories _BedCategories)
        {
            return new BedCategoriesCRUDViewModel
            {
                Id = _BedCategories.Id,
                Name = _BedCategories.Name,
                Description = _BedCategories.Description,
                CreatedDate = _BedCategories.CreatedDate,
                ModifiedDate = _BedCategories.ModifiedDate,
                CreatedBy = _BedCategories.CreatedBy,
                ModifiedBy = _BedCategories.ModifiedBy,
                Cancelled = _BedCategories.Cancelled,

            };
        }

        public static implicit operator BedCategories(BedCategoriesCRUDViewModel vm)
        {
            return new BedCategories
            {
                Id = vm.Id,
                Name = vm.Name,
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
