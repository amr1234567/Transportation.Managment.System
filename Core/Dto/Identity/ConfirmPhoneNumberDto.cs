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
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string VerifactionCode { get; set; }
        public string RealCode { get; set; }
    }
}
