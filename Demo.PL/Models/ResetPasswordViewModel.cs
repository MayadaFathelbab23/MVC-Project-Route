using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "New Password is Required")]
		[MinLength(5, ErrorMessage = "Minimum password length is 5")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is Required")]
		[Compare(nameof(Password), ErrorMessage = "confirm password does not match")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}
