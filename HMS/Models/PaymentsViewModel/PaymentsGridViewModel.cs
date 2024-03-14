using System;

namespace HMS.Models.PaymentsViewModel
{
    public class PaymentsGridViewModel
    {
        public Int64 Id { get; set; }
        public string VisitId { get; set; }
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        public double Discount { get; set; }
        public double Tax { get; set; }
        public double SubTotal { get; set; }
        public double? GrandTotal { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}


