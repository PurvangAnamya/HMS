using System;

namespace HMS.Models
{
    public class PatientVisitPaymentHistory : EntityBase
    {
        public Int64 Id { get; set; }
        public string VisitId { get; set; }
        public Int64 PaymentId { get; set; }
    }
}
