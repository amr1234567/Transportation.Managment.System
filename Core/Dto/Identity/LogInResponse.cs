using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Identity
{
    public class LogInResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public TokenModel TokenModel { get; set; }
    }
}
