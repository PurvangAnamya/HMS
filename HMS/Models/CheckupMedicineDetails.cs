﻿using System;

namespace HMS.Models
{
    public class CheckupMedicineDetails : EntityBase
    {
        public Int64 Id { get; set; }
        public string VisitId { get; set; }
        public Int64 MedicineId { get; set; }
        public int NoofDays { get; set; }
        public string WhentoTake { get; set; }
        public bool IsBeforeMeal { get; set; }
    }
}
