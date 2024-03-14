using HMS.Models.PatientTestDetailViewModel;
using System.Collections.Generic;

namespace HMS.Models.PatientTestViewModel
{
    public class ManagePatientTestViewModel
    {
        public PatientTestGridViewModel PatientTestGridViewModel { get; set; }
        public PatientTestCRUDViewModel PatientTestCRUDViewModel { get; set; }
        public List<PatientTestDetailCRUDViewModel> listPatientTestDetailCRUDViewModel { get; set; }
        public List<PatientTestResultUpdateViewModel> listPatientTestResultUpdateViewModel { get; set; }
    }
}
