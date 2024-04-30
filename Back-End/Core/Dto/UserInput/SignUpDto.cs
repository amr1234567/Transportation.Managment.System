using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserInput
{
    public class SignUpDto
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} must contain a value")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "{0} must contain a value")]
        [StringLength(30, MinimumLength = 3)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "{0} must contain a value")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} must contain a value")]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "{0} must contain a value")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
