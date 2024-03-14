using System.ComponentModel.DataAnnotations;

namespace HMS.Models.MedicineCategoriesViewModel
{
    public class MedicineCategoriesCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public static implicit operator MedicineCategoriesCRUDViewModel(MedicineCategories _MedicineCategories)
        {
            return new MedicineCategoriesCRUDViewModel
            {
                Id = _MedicineCategories.Id,
                Name = _MedicineCategories.Name,
                Description = _MedicineCategories.Description,
                CreatedDate = _MedicineCategories.CreatedDate,
                ModifiedDate = _MedicineCategories.ModifiedDate,
                CreatedBy = _MedicineCategories.CreatedBy,
                ModifiedBy = _MedicineCategories.ModifiedBy,
                Cancelled = _MedicineCategories.Cancelled,

            };
        }

        public static implicit operator MedicineCategories(MedicineCategoriesCRUDViewModel vm)
        {
            return new MedicineCategories
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

