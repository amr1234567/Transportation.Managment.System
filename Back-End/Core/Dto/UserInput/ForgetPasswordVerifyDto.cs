using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserInput
{
    public class ForgetPasswordVerifyDto
    {
        [DataType(DataType.PhoneNumber), Required]
        [RegularExpression(@"^\+20(10|15|11|12)\d{8}")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
