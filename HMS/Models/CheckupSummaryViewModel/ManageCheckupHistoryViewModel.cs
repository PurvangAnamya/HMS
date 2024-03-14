using HMS.Models.CheckupMedicineDetailsViewModel;
using HMS.Models.CompanyInfoViewModel;
using HMS.Models.PatientInfoViewModel;
using System.Collections.Generic;

namespace HMS.Models.CheckupSummaryViewModel
{
    public class ManageCheckupHistoryViewModel
    {
        public List<CheckupSummaryCRUDViewModel> listCheckupSummaryCRUDViewModel { get; set; }
        public List<CheckupMedicineDetailsCRUDViewModel> listCheckupMedicineDetailsCRUDViewModel { get; set; }
        public PatientInfoCRUDViewModel PatientInfoCRUDViewModel { get; set; }
        public CompanyInfoCRUDViewModel CompanyInfoCRUDViewModel { get; set; }
    }
}
