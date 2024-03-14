using System;

namespace HMS.Models.LabTestConfigurationViewModel
{
    public class LabTestConfigurationGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 LabTestsId { get; set; }
        public string LabTestName { get; set; }
        public string LabTestCategoryName { get; set; }
        public int Sorting { get; set; }
        public string ReportGroup { get; set; }
        public string NameOfTest { get; set; }
        public string Result { get; set; }
        public string NormalValue { get; set; }
    }
}
