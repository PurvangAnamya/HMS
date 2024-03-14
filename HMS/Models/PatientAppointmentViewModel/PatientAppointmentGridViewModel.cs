using System;

namespace HMS.Models.PatientAppointmentViewModel
{
    public class PatientAppointmentGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 PatientId { get; set; }
        public Int64 DoctorId { get; set; }
        public string SerialNo { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string Note { get; set; }
    }
}
