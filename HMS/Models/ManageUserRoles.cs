using System;

namespace HMS.Models
{
    public class ManageUserRoles : EntityBase
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }     
        public Int64 ImageId { get; set; }
        public Int64 DashboardImageId { get; set; }
    }
}
