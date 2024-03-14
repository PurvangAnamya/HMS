using System;

namespace HMS.Models
{
    public class Medicines : EntityBase
    {
        public Int64 Id { get; set; }
        public string Code { get; set; }
        public int MedicineCategoryId { get; set; }
        public string PaymentItemCode { get; set; }
        public string MedicineName { get; set; }
        public Int64 ManufactureId { get; set; }
        public Int64 UnitId { get; set; }
        public double? UnitPrice { get; set; }
        public double? SellPrice { get; set; }
        public double? OldUnitPrice { get; set; }
        public double? OldSellPrice { get; set; }
        public double? Quantity { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string UpdateQntType { get; set; }
        public string UpdateQntNote { get; set; }
        public string StockKeepingUnit { get; set; }
    }
}
