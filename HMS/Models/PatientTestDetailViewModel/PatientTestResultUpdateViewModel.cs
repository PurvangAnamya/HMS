using System;

namespace HMS.Models.PatientTestDetailViewModel
{
    public class PatientTestResultUpdateViewModel
    {
        public Int64 Id { get; set; }
        public string Result { get; set; }
        public string Remarks { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }


        public static implicit operator PatientTestResultUpdateViewModel(PatientTestDetail _PatientTestDetail)
        {
            return new PatientTestResultUpdateViewModel
            {
                Id = _PatientTestDetail.Id,
                Result = _PatientTestDetail.Result,
                Remarks = _PatientTestDetail.Remarks,
                ModifiedDate = _PatientTestDetail.ModifiedDate,
                ModifiedBy = _PatientTestDetail.ModifiedBy,
            };
        }

        public static implicit operator PatientTestDetail(PatientTestResultUpdateViewModel vm)
        {
            return new PatientTestDetail
            {
                Id = vm.Id,
                Result = vm.Result,
                Remarks = vm.Remarks,
                ModifiedDate = vm.ModifiedDate,
                ModifiedBy = vm.ModifiedBy,
            };
        }
    }
}
