using System;

namespace HMS.Models.BedViewModel
{
    public class BedGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 BedCategoryId { get; set; }
        public string BedCategoryName { get; set; }
        public double? BedCategoryPrice { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public string Hospital { get; set; }
    }
}
