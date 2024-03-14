using System.ComponentModel.DataAnnotations;

namespace HMS.Models.LabTestCategoriesViewModel
{
    public class LabTestCategoriesCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }



        public static implicit operator LabTestCategoriesCRUDViewModel(LabTestCategories _LabTestCategories)
        {
            return new LabTestCategoriesCRUDViewModel
            {
                Id = _LabTestCategories.Id,
                Name = _LabTestCategories.Name,
                Description = _LabTestCategories.Description,
                CreatedDate = _LabTestCategories.CreatedDate,
                ModifiedDate = _LabTestCategories.ModifiedDate,
                CreatedBy = _LabTestCategories.CreatedBy,
                ModifiedBy = _LabTestCategories.ModifiedBy,
                Cancelled = _LabTestCategories.Cancelled,
            };
        }

        public static implicit operator LabTestCategories(LabTestCategoriesCRUDViewModel vm)
        {
            return new LabTestCategories
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
