using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage ="Username is required")]
        public string Username { get; set; }

		[Required(ErrorMessage ="First name is required")]
        public string FName { get; set; }

		[Required(ErrorMessage = "Last name is required")]
		public string LName { get; set; }

        [Required(ErrorMessage ="Email is Required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
        public string  Email { get; set; }

		[Required(ErrorMessage ="Password is Required")]
		[MinLength(5 , ErrorMessage ="Minimum password length is 5")]
		[DataType(DataType.Password)]
        public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is Required")]
		[Compare(nameof(Password) , ErrorMessage ="confirm password does not match")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}
