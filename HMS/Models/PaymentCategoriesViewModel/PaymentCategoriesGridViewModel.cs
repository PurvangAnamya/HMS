using System;

namespace HMS.Models.PaymentCategoriesViewModel
{
    public class PaymentCategoriesGridViewModel : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public string Description { get; set; }
        public string Hospital { get; set; }
    }
}
