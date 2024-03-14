using System;

namespace HMS.Models
{
    public class DoctorsInfo : EntityBase
    {
        public Int64 Id { get; set; }
        public string ApplicationUserId { get; set; }
        public Int64 DesignationId { get; set; }
        public string DoctorsID { get; set; }
        public double? DoctorFee { get; set; }
        public Int64 RoleId { get; set; }
    }
}
