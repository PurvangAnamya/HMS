using System;

namespace HMS.Models.MedicinesViewModel
{
    public class UpdateMedicineInventoryViewModel
    {
        public string PaymentItemCode { get; set; }
        public int TranQuantity { get; set; }
        public bool IsAddition { get; set; }
        public string Action { get; set; }
        public string CurrentUserName { get; set; }

        public static implicit operator UpdateMedicineInventoryViewModel(PaymentsDetails _PaymentsDetails)
        {
            return new UpdateMedicineInventoryViewModel
            {
                PaymentItemCode = _PaymentsDetails.PaymentItemCode,
                TranQuantity = _PaymentsDetails.Quantity
            };
        }

        public static implicit operator PaymentsDetails(UpdateMedicineInventoryViewModel vm)
        {
            return new PaymentsDetails
            {
                PaymentItemCode = vm.PaymentItemCode,
                Quantity = vm.TranQuantity
            };
        }
    }
}
