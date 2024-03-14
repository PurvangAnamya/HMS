using System;

namespace HMS.Models.PaymentsDetailsViewModel
{
    public class PaymentsDetailsUpdateViewModel
    {
        public Int64 Id { get; set; }
        public int Quantity { get; set; }
        public double? UnitPrize { get; set; }
        public double? TotalAmount { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }


        public static implicit operator PaymentsDetailsUpdateViewModel(PaymentsDetails _PaymentsDetails)
        {
            return new PaymentsDetailsUpdateViewModel
            {
                Id = _PaymentsDetails.Id,
                Quantity = _PaymentsDetails.Quantity,
                UnitPrize = _PaymentsDetails.UnitPrize,
                TotalAmount = _PaymentsDetails.TotalAmount,
                ModifiedDate = _PaymentsDetails.ModifiedDate,
                ModifiedBy = _PaymentsDetails.ModifiedBy,
            };
        }

        public static implicit operator PaymentsDetails(PaymentsDetailsUpdateViewModel vm)
        {
            return new PaymentsDetails
            {
                Id = vm.Id,
                Quantity = vm.Quantity,
                UnitPrize = vm.UnitPrize,
                TotalAmount = vm.TotalAmount,
                ModifiedDate = vm.ModifiedDate,
                ModifiedBy = vm.ModifiedBy,
            };
        }
    }
}
