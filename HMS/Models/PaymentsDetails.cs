using System;

namespace HMS.Models
{
    public class PaymentsDetails : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 PaymentsId { get; set; }
        public Int64 ItemDetailId { get; set; }
        public string PaymentItemCode { get; set; }
        public int Quantity { get; set; }
        public double? UnitPrize { get; set; }
        public double? TotalAmount { get; set; }
    }
}
