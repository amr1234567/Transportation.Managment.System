using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Identity
{
    public class ResetPasswordDto
    {
        [Required]
        [Length(6, 6)]
        public string code { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} must contain a value")]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} must contain a value")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#$%^&*()-+=])(.{8,})$")]
        [DefaultValue("@Aa123456789")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "{0} must contain a value")]
        [Compare(nameof(Password))]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#$%^&*()-+=])(.{8,})$")]
        [DefaultValue("@Aa123456789")]
        public string ConfirmPassword { get; set; }
    }
}
