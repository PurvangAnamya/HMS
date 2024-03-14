using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.PaymentsDetailsViewModel
{
    public class PaymentsDetailsCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public Int64 PaymentsId { get; set; }
        public Int64 ItemDetailId { get; set; }
        [Display(Name = "Payment Categories")]
        [Required]
        public string PaymentItemCode { get; set; }
        public string PaymentItemName { get; set; }
        public int Quantity { get; set; }
        public double? UnitPrize { get; set; }
        public double? TotalAmount { get; set; }



        public static implicit operator PaymentsDetailsCRUDViewModel(PaymentsDetails _PaymentsDetails)
        {
            return new PaymentsDetailsCRUDViewModel
            {
                Id = _PaymentsDetails.Id,
                PaymentsId = _PaymentsDetails.PaymentsId,
                ItemDetailId = _PaymentsDetails.ItemDetailId,
                PaymentItemCode = _PaymentsDetails.PaymentItemCode,
                Quantity = _PaymentsDetails.Quantity,
                UnitPrize = _PaymentsDetails.UnitPrize,
                TotalAmount = _PaymentsDetails.TotalAmount,
                CreatedDate = _PaymentsDetails.CreatedDate,
                ModifiedDate = _PaymentsDetails.ModifiedDate,
                CreatedBy = _PaymentsDetails.CreatedBy,
                ModifiedBy = _PaymentsDetails.ModifiedBy,
                Cancelled = _PaymentsDetails.Cancelled,
            };
        }

        public static implicit operator PaymentsDetails(PaymentsDetailsCRUDViewModel vm)
        {
            return new PaymentsDetails
            {
                Id = vm.Id,
                PaymentsId = vm.PaymentsId,
                ItemDetailId = vm.ItemDetailId,
                PaymentItemCode = vm.PaymentItemCode,
                Quantity = vm.Quantity,
                UnitPrize = vm.UnitPrize,
                TotalAmount = vm.TotalAmount,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
