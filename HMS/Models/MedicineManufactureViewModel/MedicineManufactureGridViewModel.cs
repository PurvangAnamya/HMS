using System;

namespace HMS.Models.MedicineManufactureViewModel
{
    public class MedicineManufactureGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Hospital { get; set; }

    }
}
