using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers.Classes
{
    public class TwilioConfiguration
    {
        public string AccountSID { get; set; }
        public string AuthToken { get; set; }
        public string VerificationServiceSID { get; set; }
        public string PhoneNumber { get; set; }
    }

}
