using System;

namespace HMS.Models
{
    public class PatientTestDetail : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 PatientTestId { get; set; }
        public Int64 LabTestsId { get; set; }
        public string Result { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string Remarks { get; set; }
    }
}
