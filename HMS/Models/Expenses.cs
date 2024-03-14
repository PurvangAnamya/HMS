using System;

namespace HMS.Models
{
    public class Expenses : EntityBase
    {
        public Int64 Id { get; set; }
        public int ExpenseCategoriesId { get; set; }
        public double Amount { get; set; }
    }
}
