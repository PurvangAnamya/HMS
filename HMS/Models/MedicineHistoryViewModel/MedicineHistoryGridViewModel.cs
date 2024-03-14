using System;

namespace HMS.Models.MedicineHistoryViewModel
{
    public class MedicineHistoryGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 MedicineId { get; set; }
        public string Code { get; set; }
        public string MedicineName { get; set; }
        public Int64 ManufactureId { get; set; }
        public string Unit { get; set; }
        public double? UnitPrice { get; set; }
        public double? SellPrice { get; set; }
        public double? OldUnitPrice { get; set; }
        public double? OldSellPrice { get; set; }
        public int OldQuantity { get; set; }
        public int NewQuantity { get; set; }
        public int TranQuantity { get; set; }
        public string UpdateQntType { get; set; }
        public string UpdateQntNote { get; set; }
        public string Note { get; set; }
        public string Action { get; set; }
    }
}

