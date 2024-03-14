using System;

namespace HMS.Models
{
    public class PatientInfo : EntityBase
    {
        public Int64 Id { get; set; }
        public string PatientCode { get; set; }
        public string ApplicationUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public string SpouseName { get; set; }
        public string BloodGroup { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public double? RegistrationFee { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public bool? Agreement { get; set; }
        public string Remarks { get; set; }
        public string ProfilePicture { get; set; }
    }
}
