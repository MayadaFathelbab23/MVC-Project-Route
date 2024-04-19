using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class UserEditViewModel
    {
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
