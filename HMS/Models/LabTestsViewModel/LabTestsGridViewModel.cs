using System;

namespace HMS.Models.LabTestsViewModel
{
    public class LabTestsGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 LabTestCategoryId { get; set; }
        public string LabTestCategoryName { get; set; }
        public string LabTestName { get; set; }
        public string Unit { get; set; }
        public string ReferenceRange { get; set; }
        public bool Status { get; set; }
    }
}
