using System;

namespace HMS.Models.MedicinesViewModel
{
    public class MedicinesGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public int MedicineCategoryId { get; set; }
        public string MedicineCategoryName { get; set; }
        public string MedicineName { get; set; }
        public double? UnitPrice { get; set; }
        public string Description { get; set; }
    }
}
