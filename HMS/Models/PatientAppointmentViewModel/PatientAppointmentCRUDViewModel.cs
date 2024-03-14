using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.PatientAppointmentViewModel
{
    public class PatientAppointmentCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Patient Name")]
        [Required]
        public Int64 PatientId { get; set; }
        public string VisitId { get; set; }
        [Display(Name = "Patient Type")]
        [Required]
        public string PatientType { get; set; }
        public string PatientName { get; set; }
        [Display(Name = "Doctor Name")]
        [Required]
        public Int64 DoctorId { get; set; }
        public string DoctorName { get; set; }
        [Display(Name = "Serial No")]
        [Required]
        public Int64 SerialNo { get; set; }
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; } = DateTime.Today;
        [Display(Name = "Appointment Time")]
        public DateTime AppointmentTime { get; set; } = DateTime.Now;
        public string AppointmentTimeDisplay { get; set; }
        public string Note { get; set; }


        public static implicit operator PatientAppointmentCRUDViewModel(PatientAppointment _PatientAppointment)
        {
            return new PatientAppointmentCRUDViewModel
            {
                Id = _PatientAppointment.Id,
                PatientId = _PatientAppointment.PatientId,
                VisitId = _PatientAppointment.VisitId,
                PatientType = _PatientAppointment.PatientType,
                DoctorId = _PatientAppointment.DoctorId,
                SerialNo = _PatientAppointment.SerialNo,
                AppointmentDate = _PatientAppointment.AppointmentDate,
                AppointmentTime = _PatientAppointment.AppointmentTime,
                Note = _PatientAppointment.Note,
                CreatedDate = _PatientAppointment.CreatedDate,
                ModifiedDate = _PatientAppointment.ModifiedDate,
                CreatedBy = _PatientAppointment.CreatedBy,
                ModifiedBy = _PatientAppointment.ModifiedBy,
                Cancelled = _PatientAppointment.Cancelled,
            };
        }

        public static implicit operator PatientAppointment(PatientAppointmentCRUDViewModel vm)
        {
            return new PatientAppointment
            {
                Id = vm.Id,
                PatientId = vm.PatientId,
                VisitId = vm.VisitId,
                PatientType = vm.PatientType,
                DoctorId = vm.DoctorId,
                SerialNo = vm.SerialNo,
                AppointmentDate = vm.AppointmentDate,
                AppointmentTime = vm.AppointmentTime,
                Note = vm.Note,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
