using HMS.Models.CheckupMedicineDetailsViewModel;
using System;
using System.Collections.Generic;

namespace HMS.Models.MedicineLabelViewModel
{
    public class MedicineLabelPrintViewModel : EntityBase
    {
        public Int64 PatientId { get; set; }
        public string HospitalName { get; set; }
        public string PatientName { get; set; }
        public List<CheckupMedicineDetailsCRUDViewModel> listCheckupMedicineDetailsCRUDViewModel { get; set; }
    }
}

