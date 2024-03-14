using System;

namespace HMS.Models
{
    public class Bed : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 BedCategoryId { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
    }
}
