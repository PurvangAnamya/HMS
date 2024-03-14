using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.PaymentsViewModel
{
    public class PaymentsCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Patient")]
        [Required]
        public Int64 PatientId { get; set; }
        public string VisitId { get; set; }
        public string InvoiceNo { get; set; }
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        [Display(Name = "Common Charge")]
        public double CommonCharge { get; set; }
        [Display(Name = "Discount(%)")]
        public double Discount { get; set; }
        public double DiscountAmount { get; set; }
        [Display(Name = "Tax(%)")]
        public double Tax { get; set; }
        public double TaxAmount { get; set; }
        [Display(Name = "Sub Total")]
        public double SubTotal { get; set; }
        [Display(Name = "Grand Total")]
        public double? GrandTotal { get; set; }
        [Display(Name = "Paid Amount")]
        public double PaidAmount { get; set; }
        [Display(Name = "Due Amount")]
        public double DueAmount { get; set; }
        [Display(Name = "Changed Amount")]
        public double ChangedAmount { get; set; }
        [Display(Name = "Insurance No")]
        public string InsuranceNo { get; set; }
        [Display(Name = "Company Name")]
        public Int64? InsuranceCompanyId { get; set; }
        [Display(Name = "Company Name")]
        public string InsuranceCompanyName { get; set; }
        [Display(Name = "Insurance(%)")]
        public double InsuranceCoverage { get; set; }
        public double? InsuranceAmount { get; set; }
        [Display(Name = "Currency")]
        public Int64 CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public int MedicineSellState { get; set; } = 0;
        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; }
        public string CurrentURL { get; set; }
        public string UserRole { get; set; }

        public static implicit operator PaymentsCRUDViewModel(Payments _Payments)
        {
            return new PaymentsCRUDViewModel
            {
                Id = _Payments.Id,
                PatientId = _Payments.PatientId,
                VisitId = _Payments.VisitId,
                InvoiceNo = _Payments.InvoiceNo,
                CommonCharge = _Payments.CommonCharge,
                Discount = _Payments.Discount,
                DiscountAmount = _Payments.DiscountAmount,
                Tax = _Payments.Tax,
                TaxAmount = _Payments.TaxAmount,
                SubTotal = _Payments.SubTotal,
                GrandTotal = _Payments.GrandTotal,
                PaidAmount = _Payments.PaidAmount,
                DueAmount = _Payments.DueAmount,
                ChangedAmount = _Payments.ChangedAmount,
                InsuranceNo = _Payments.InsuranceNo,
                InsuranceCompanyId = _Payments.InsuranceCompanyId,
                InsuranceCoverage = _Payments.InsuranceCoverage,
                CurrencyId = _Payments.CurrencyId,
                MedicineSellState = _Payments.MedicineSellState,
                PaymentStatus = _Payments.PaymentStatus,

                CreatedDate = _Payments.CreatedDate,
                ModifiedDate = _Payments.ModifiedDate,
                CreatedBy = _Payments.CreatedBy,
                ModifiedBy = _Payments.ModifiedBy,
                Cancelled = _Payments.Cancelled,
            };
        }

        public static implicit operator Payments(PaymentsCRUDViewModel vm)
        {
            return new Payments
            {
                Id = vm.Id,
                PatientId = vm.PatientId,
                VisitId = vm.VisitId,
                InvoiceNo = vm.InvoiceNo,
                CommonCharge = vm.CommonCharge,
                Discount = vm.Discount,
                DiscountAmount = vm.DiscountAmount,
                Tax = vm.Tax,
                TaxAmount = vm.TaxAmount,
                SubTotal = vm.SubTotal,
                GrandTotal = vm.GrandTotal,
                PaidAmount = vm.PaidAmount,
                DueAmount = vm.DueAmount,
                ChangedAmount = vm.ChangedAmount,
                InsuranceNo = vm.InsuranceNo,
                InsuranceCompanyId = vm.InsuranceCompanyId,
                InsuranceCoverage = vm.InsuranceCoverage,
                CurrencyId = vm.CurrencyId,
                MedicineSellState = vm.MedicineSellState,
                PaymentStatus = vm.PaymentStatus,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}


