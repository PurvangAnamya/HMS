using System;

namespace HMS.Models.PaymentsViewModel
{
    public class PatientPaymentViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 PatientId { get; set; }
        public Int64 PaymentId { get; set; }
        public string VisitId { get; set; }
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        public Int64 DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime CheckupDate { get; set; }
    }
}


