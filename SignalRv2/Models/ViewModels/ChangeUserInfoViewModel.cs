using SignalRv2.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Models.ViewModels
{
    public class ChangeUserInfoViewModel
    {
        [Required(ErrorMessage = "Please enter a FirstName")]
        [StringLength(Constants.MaxNameLength, ErrorMessage = "{0} must be between {2} and {1} character", MinimumLength = Constants.MinNameLength)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a LastName")]
        [StringLength(Constants.MaxNameLength, ErrorMessage = "{0} must be between {2} and {1} character", MinimumLength = Constants.MinNameLength)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        public string AvatarUrl { get; set; } = "default";
    }
}
