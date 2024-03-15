
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class UserImages :EntityBase
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }     

       
      
    }
}
