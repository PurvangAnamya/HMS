using System;

namespace HMS.Models.PatientTestDetailViewModel
{
    public class PatientTestDetailGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 PatientTestId { get; set; }
        public Int64 LabTestsId { get; set; }
        public string LabTestsName { get; set; }
        public string Result { get; set; }
        public string Remarks { get; set; }
    }
}