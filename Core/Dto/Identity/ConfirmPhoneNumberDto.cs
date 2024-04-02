using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Identity
{
    public class ConfirmPhoneNumberDto
    {
        public string PhoneNumber { get; set; }
        public string VerifactionCode { get; set; }
        public string RealCode { get; set; }
    }
}
