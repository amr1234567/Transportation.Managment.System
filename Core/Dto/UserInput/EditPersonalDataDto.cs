using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserInput
{
    public class EditPersonalDataDto
    {
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
