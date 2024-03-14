using System;

namespace HMS.Models.ReportViewModel
{
    public class ServicePaymentViewModel
    {
        public Int64 Id { get; set; }
        public Int64 PaymentId { get; set; }
        public Int64 ItemDetailId { get; set; }
        public string PaymentItemCode { get; set; }
        public string PatientName { get; set; }
        public string PaymentItemName { get; set; }
        public int Quantity { get; set; }
        public double? UnitPrize { get; set; }
        public double? TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
