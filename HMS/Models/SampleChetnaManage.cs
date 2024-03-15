
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class SampleChetnaManage : EntityBase
    {
        public Int64 Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }
      

        [Display(Name = "Profile Picture")]      
        public string ProfilePicture { get; set; } // for Old Method
        public Int64 ImageId { get; set; }
    }
}
