namespace HMS.Models
{
    public class UserRoleCountsModel : EntityBase
    {
        public int Id { get; set; }
        public Int64 RoleId { get; set; }
        public string RoleName { get; set; }
        public Int64 UserCounts { get; set; }
        public Int64 ImageId { get; set; }
        public Int64 DashboardImageId { get; set; }
        public string DashboardImage { get; set; }
        public string LeftMenuImage { get; set; }
    }
}
