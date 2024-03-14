using System;

namespace HMS.Models.PaymentsDetailsViewModel
{
    public class PaymentsDetailsGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 PaymentsId { get; set; }
        public Int64 PaymentCategoriesId { get; set; }
        public string PaymentCategoriesName { get; set; }
        public double ChargeAmount { get; set; }
    }
}

