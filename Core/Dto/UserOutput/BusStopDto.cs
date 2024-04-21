using Core.Identity;
using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserOutput
{
    public class BusStopDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IEnumerable<BusStopDto> BusStops { get; set; }
    }
}
