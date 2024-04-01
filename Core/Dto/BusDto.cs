using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class BusDto
    {
        [Required]
        [Range(25, 40)]
        public int NumberOfSeats { get; set; }
    }
}
