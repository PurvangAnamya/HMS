using System;

namespace HMS.Models.CheckupSummaryViewModel
{
    public class CheckupSummaryGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 PatientId { get; set; }
        public string VisitId { get; set; }
        public Int64 DoctorId { get; set; }
        public string PatientType { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string HPI { get; set; }
        public string VitalSigns { get; set; }
        public string PhysicalExamination { get; set; }
        public string Comments { get; set; }
        public DateTime CheckupDate { get; set; }
        public DateTime NextVisitDate { get; set; }
        public string Advice { get; set; }
        public int BPSystolic { get; set; }
        public int BPDiastolic { get; set; }
        public int RespirationRate { get; set; }
        public int Temperature { get; set; }
        public string NursingNotes { get; set; }
    }
}
