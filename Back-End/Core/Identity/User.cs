using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "{0} must contain a value")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Bus Stop Name")]
        public string Name { get; set; }
    }
}
