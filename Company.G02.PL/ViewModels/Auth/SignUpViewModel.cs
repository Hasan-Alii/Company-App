using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.ViewModels.Auth
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "User Name is Required!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "First Name is Required!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required!")]
        [EmailAddress(ErrorMessage = "Invalid Email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required!")]
        [DataType(DataType.Password)]
		public string Password { get; set; }
        
        [Required(ErrorMessage = "Confirmed Password is Required!")]
		[DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirmed Password Doesn't match Password ")]
		public string ConfirmPassword { get; set; }        
		public bool IsAgree { get; set; }
	}
}
