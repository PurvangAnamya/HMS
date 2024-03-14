using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.PaymentModeHistoryViewModel
{
    public class PaymentModeHistoryCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public Int64 PaymentId { get; set; }
        public string ModeOfPayment { get; set; }
        public double? Amount { get; set; }
        public string ReferenceNo { get; set; }

        public static implicit operator PaymentModeHistoryCRUDViewModel(PaymentModeHistory _PaymentModeHistory)
        {
            return new PaymentModeHistoryCRUDViewModel
            {
                Id = _PaymentModeHistory.Id,
                PaymentId = _PaymentModeHistory.PaymentId,
                ModeOfPayment = _PaymentModeHistory.ModeOfPayment,
                Amount = _PaymentModeHistory.Amount,
                ReferenceNo = _PaymentModeHistory.ReferenceNo,
                CreatedDate = _PaymentModeHistory.CreatedDate,
                ModifiedDate = _PaymentModeHistory.ModifiedDate,
                CreatedBy = _PaymentModeHistory.CreatedBy,
                ModifiedBy = _PaymentModeHistory.ModifiedBy,
                Cancelled = _PaymentModeHistory.Cancelled
            };
        }

        public static implicit operator PaymentModeHistory(PaymentModeHistoryCRUDViewModel vm)
        {
            return new PaymentModeHistory
            {
                Id = vm.Id,
                PaymentId = vm.PaymentId,
                ModeOfPayment = vm.ModeOfPayment,
                Amount = vm.Amount,
                ReferenceNo = vm.ReferenceNo,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled
            };
        }
    }
}
