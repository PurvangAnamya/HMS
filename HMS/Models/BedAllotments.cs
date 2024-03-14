using System;

namespace HMS.Models
{
    public class BedAllotments : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 PatientId { get; set; }
        public Int64 BedCategoryId { get; set; }
        public Int64 BedId { get; set; }
        public bool IsReleased { get; set; }
        public DateTime AllotmentDate { get; set; }
        public DateTime DischargeDate { get; set; }
        public string Note { get; set; }
    }
}
