using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ResponseModel<T>
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Body { get; set; }
    }
}
