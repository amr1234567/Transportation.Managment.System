using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserOutput
{
    public class ErrorModelState
    {
        public ErrorModelState(string fieldName, IEnumerable<string> errors = null)
        {
            FieldName = fieldName;
            Errors = errors ?? [];
        }

        public string FieldName { get; set; }
        public IEnumerable<string> Errors { get; set; } = [];
    }
}
