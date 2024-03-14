using System;

namespace HMS.Models.ReportViewModel
{
    public class PaymentModeReportViewModel
    {
        public Int64 Id { get; set; }
        public Int64 PaymentId { get; set; }
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        public string Insurance { get; set; }
        public string PaymentStatus { get; set; }
        public string ModeofPayment { get; set; }
        public double? Amount { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalPaidAmount { get; set; }
        public double? TotalDueAmount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
