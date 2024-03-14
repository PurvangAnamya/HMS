namespace HMS.Models.HospitalViewModel
{
    public class HospitalViewModel: EntityBase
    {
        public Int64 Id { get; set; }
        public string HospitalName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string HospitalLogoImagePath { get; set; }
    }
}
