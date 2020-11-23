using SignalRv2.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Please enter a Password")]
        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter a Password")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [StringLength(Constants.MaxPassLength, ErrorMessage = "{0} must be between {2} and {1} character", MinimumLength = Constants.MinPassLength)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please repeat your password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
