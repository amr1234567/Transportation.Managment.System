using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class BusStopDto
    {
        public string Name { get; set; }
        public List<Bus> buses { get; set; }
    }
}
