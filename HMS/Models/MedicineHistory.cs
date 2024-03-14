using System;

namespace HMS.Models
{
    public class MedicineHistory : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 MedicineId { get; set; }
        public string Code { get; set; }
        public string MedicineName { get; set; }
        public Int64 ManufactureId { get; set; }
        public Int64 UnitId { get; set; }
        public double? UnitPrice { get; set; }
        public double? SellPrice { get; set; }
        public double? OldUnitPrice { get; set; }
        public double? OldSellPrice { get; set; }
        public double? OldQuantity { get; set; }
        public double? NewQuantity { get; set; }
        public int TranQuantity { get; set; }
        public string UpdateQntType { get; set; }
        public string UpdateQntNote { get; set; }
        public string Note { get; set; }
        public string Action { get; set; }
    }
}