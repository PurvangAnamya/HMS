using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.ExpensesViewModel
{
    public class ExpensesCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Expense Categories")]
        [Required]
        public int ExpenseCategoriesId { get; set; }
        [Required]
        public double Amount { get; set; }



        public static implicit operator ExpensesCRUDViewModel(Expenses _Expenses)
        {
            return new ExpensesCRUDViewModel
            {
                Id = _Expenses.Id,
                ExpenseCategoriesId = _Expenses.ExpenseCategoriesId,
                Amount = _Expenses.Amount,
                CreatedDate = _Expenses.CreatedDate,
                ModifiedDate = _Expenses.ModifiedDate,
                CreatedBy = _Expenses.CreatedBy,
                ModifiedBy = _Expenses.ModifiedBy,
                Cancelled = _Expenses.Cancelled,

            };
        }

        public static implicit operator Expenses(ExpensesCRUDViewModel vm)
        {
            return new Expenses
            {
                Id = vm.Id,
                ExpenseCategoriesId = vm.ExpenseCategoriesId,
                Amount = vm.Amount,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,

            };
        }
    }
}
