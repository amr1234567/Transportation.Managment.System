using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Dto.Identity
{
    public class TokenModel
    {
        public string? Token { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
