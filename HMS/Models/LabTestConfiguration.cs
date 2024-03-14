using System;

namespace HMS.Models
{
    public class LabTestConfiguration : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 LabTestsId { get; set; }
        public int? Sorting { get; set; }
        public string ReportGroup { get; set; }
        public string NameOfTest { get; set; }
        public string Result { get; set; }
        public string NormalValue { get; set; }
    }
}
