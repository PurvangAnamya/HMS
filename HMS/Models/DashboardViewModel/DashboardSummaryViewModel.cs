using System;

namespace HMS.Models.DashboardViewModel
{
    public class DashboardSummaryViewModel
    {
        public Int64 TotalPatient { get; set; }
        public Int64 TotalDoctor { get; set; }
        public Int64 TotalNurse { get; set; }
        public Int64 TotalPharmacist { get; set; }
        public Int64 TotalLaboratorist { get; set; }
        public Int64 TotalAccountant { get; set; }
        public Int64 TotalMedicines { get; set; }
        public Int64 TotalBeds { get; set; }
    }
}
