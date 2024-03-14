using System;

namespace HMS.Models.ExpensesViewModel
{
    public class ExpensesGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public int ExpenseCategoriesId { get; set; }
        public string ExpenseCategoriesName { get; set; }
        public double Amount { get; set; }
    }
}
