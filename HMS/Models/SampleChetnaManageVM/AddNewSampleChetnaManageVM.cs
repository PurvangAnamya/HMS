using System.ComponentModel.DataAnnotations;

namespace HMS.Models.SampleChetnaManageVM
{
    public class AddNewSampleChetnaManageVM
    {
        public Int64 Id { get; set; }
        [Display(Name = "Role Name"), Required]
        public string RoleName { get; set; }
    }
}
