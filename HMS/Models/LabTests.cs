using System;

namespace HMS.Models
{
    public class LabTests : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 LabTestCategoryId { get; set; }
        public string PaymentItemCode { get; set; }
        public string LabTestName { get; set; }
        public string Unit { get; set; }
        public double UnitPrice { get; set; }
        public string ReferenceRange { get; set; }
        public bool Status { get; set; }
    }
}
