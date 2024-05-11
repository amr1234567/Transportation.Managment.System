using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Identity
{
    public class ConfirmPhoneNumberDto
    {
        [Required, DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+20(10|15|11|12)\d{8}")]
        public string PhoneNumber { get; set; }

        [EmailAddress, Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public string Email { get; set; }

        [Length(6, 6)]
        [Required]
        public string VerificationCode { get; set; }
    }
}
