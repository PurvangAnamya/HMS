using System;

namespace HMS.Models
{
    public class PatientTest : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 PatientId { get; set; }
        public string VisitId { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime TestDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string PaymentStatus { get; set; }
    }
}
