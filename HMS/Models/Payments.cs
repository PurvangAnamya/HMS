using System;

namespace HMS.Models
{
    public class Payments : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 PatientId { get; set; }
        public string VisitId { get; set; }
        public string InvoiceNo { get; set; }
        public double CommonCharge { get; set; }
        public double Discount { get; set; }
        public double DiscountAmount { get; set; }
        public double Tax { get; set; }
        public double TaxAmount { get; set; }
        public double SubTotal { get; set; }
        public double? GrandTotal { get; set; }
        public double PaidAmount { get; set; }
        public double DueAmount { get; set; }
        public double ChangedAmount { get; set; }
        public string ModeOfPayment { get; set; } //
        public string PaymentRefNo { get; set; } //
        public string InsuranceNo { get; set; }
        public Int64? InsuranceCompanyId { get; set; }
        public double InsuranceCoverage { get; set; }
        public Int64 CurrencyId { get; set; }
        public int MedicineSellState { get; set; }
        public string PaymentStatus { get; set; }
    }
}
