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
        string PhoneNumber { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
