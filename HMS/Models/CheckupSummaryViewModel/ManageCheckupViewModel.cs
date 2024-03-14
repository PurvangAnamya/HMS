using HMS.Models.CheckupMedicineDetailsViewModel;
using HMS.Models.CompanyInfoViewModel;
using HMS.Models.PatientInfoViewModel;
using HMS.Models.PatientTestDetailViewModel;
using HMS.Models.PatientTestViewModel;
using HMS.Models.VitalSignsViewModel;
using System.Collections.Generic;

namespace HMS.Models.CheckupSummaryViewModel
{
    public class ManageCheckupViewModel
    {
        public CheckupSummaryCRUDViewModel CheckupSummaryCRUDViewModel { get; set; }
        public VitalSignsCRUDViewModel VitalSignsCRUDViewModel { get; set; }
        public CheckupMedicineDetailsCRUDViewModel CheckupMedicineDetailsCRUDViewModel { get; set; }
        public List<CheckupMedicineDetailsCRUDViewModel> listCheckupMedicineDetailsCRUDViewModel { get; set; }

        public PatientTestCRUDViewModel PatientTestCRUDViewModel { get; set; }
        public List<PatientTestDetailCRUDViewModel> listPatientTestDetailCRUDViewModel { get; set; }

        public PatientInfoCRUDViewModel PatientInfoCRUDViewModel { get; set; }
        public CompanyInfoCRUDViewModel CompanyInfoCRUDViewModel { get; set; }
    }
}
