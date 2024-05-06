using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserInput
{
    public class ChangePhoneNumberDto
    {
        [Length(6, 6)]
        [Required]
        public string Verifytoken { get; set; }

        [RegularExpression(@"^\+20(10|15|11|12)\d{8}")]
        [DefaultValue("+201152899886")]
        [StringLength(13)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} must contain a value")]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public string Email { get; set; }
    }
}
