using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers.Classes
{
    public class JwtHelper
    {
        public int expirePeriodInMinuts { get; set; }
        public string issuer { get; set; }
        public string Key { get; set; }
    }
}
