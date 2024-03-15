using System;

namespace HMS.Models.BedCategoriesViewModel
{
    public class BedCategoriesGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Hospital { get; set; }
    }
}
