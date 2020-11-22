using SignalRv2.Abstract;
using System.ComponentModel.DataAnnotations;

namespace SignalRv2.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter a username")]
        [RegularExpression(@"^(?=[a-zA-Z0-9._]{" + Constants.MinUserNameLength + "," + Constants.MaxUserNameLength + "}$)(?!.*[_.]{2})[^_.].*[^_.]$", 
        ErrorMessage = "Username must be between " + Constants.MinUserNameLength +" and " + Constants.MaxUserNameLength + " character")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [StringLength(Constants.MaxPassLength, ErrorMessage = "{0} must be between {2} and {1} character", MinimumLength = Constants.MinPassLength)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]      
        public string Password { get; set; }


        [Required(ErrorMessage = "Please repeat your password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter an email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a FirstName")]
        [StringLength(Constants.MaxNameLength, ErrorMessage = "{0} must be between {2} and {1} character", MinimumLength = Constants.MinNameLength)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a LastName")]
        [StringLength(Constants.MaxNameLength, ErrorMessage = "{0} must be between {2} and {1} character", MinimumLength = Constants.MinNameLength)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
