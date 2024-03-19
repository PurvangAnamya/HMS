using System;

namespace HMS.Models.BedAllotmentsViewModel
{
    public class BedAllotmentsGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 BedCategoryId { get; set; }
        public string BedCategoryName { get; set; }
        public string PatientCode { get; set; }
        public string PatientName { get; set; }
        public Int64 BedId { get; set; }
        public double? BedCategoryPrice { get; set; }
        public string BedNo { get; set; }
        public bool IsReleased { get; set; }
        public string ReleasedStatus { get; set; }
        public string AllotmentDateDisplay { get; set; }
        public string DischargeDateDisplay { get; set; }
        public string Note { get; set; }
    }
}
