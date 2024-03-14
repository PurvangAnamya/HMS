using System;

namespace HMS.Models.ReportViewModel
{
    public class ServicePaymentSummaryViewModel
    {
        public double? LabTotal { get; set; }
        public double? MedicineTotal { get; set; }
        public double? CommonTotal { get; set; }
        public double? AllTotal { get; set; }
    }
}
