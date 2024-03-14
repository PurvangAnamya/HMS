using System;

namespace HMS.Models
{
    public class PaymentCategories : EntityBase
    {
        public int Id { get; set; }
        public string PaymentItemCode { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public string Description { get; set; }
    }
}
