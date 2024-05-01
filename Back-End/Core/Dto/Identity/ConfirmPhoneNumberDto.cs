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
        [Required,DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [EmailAddress,Required]
        public string Email { get; set; }
        public string VerifactionCode { get; set; }
    }
}
