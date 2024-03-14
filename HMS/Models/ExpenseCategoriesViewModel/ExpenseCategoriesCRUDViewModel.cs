using System.ComponentModel.DataAnnotations;

namespace HMS.Models.ExpenseCategoriesViewModel
{
    public class ExpenseCategoriesCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }



        public static implicit operator ExpenseCategoriesCRUDViewModel(ExpenseCategories _ExpenseCategories)
        {
            return new ExpenseCategoriesCRUDViewModel
            {
                Id = _ExpenseCategories.Id,
                Name = _ExpenseCategories.Name,
                Description = _ExpenseCategories.Description,
                CreatedDate = _ExpenseCategories.CreatedDate,
                ModifiedDate = _ExpenseCategories.ModifiedDate,
                CreatedBy = _ExpenseCategories.CreatedBy,
                ModifiedBy = _ExpenseCategories.ModifiedBy,
                Cancelled = _ExpenseCategories.Cancelled,

            };
        }

        public static implicit operator ExpenseCategories(ExpenseCategoriesCRUDViewModel vm)
        {
            return new ExpenseCategories
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
