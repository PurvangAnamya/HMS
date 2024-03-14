using System;

namespace HMS.Models
{
    public class PatientAppointment : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 PatientId { get; set; }
        public string VisitId { get; set; }
        public string PatientType { get; set; }
        public Int64 DoctorId { get; set; }
        public Int64 SerialNo { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string Note { get; set; }
    }
}
