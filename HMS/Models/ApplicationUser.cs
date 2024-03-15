using Microsoft.AspNetCore.Identity;

namespace HMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Int64 Hospitalid { get; set; }
        //public Hospital Hospital { get; set; }
    }
}
