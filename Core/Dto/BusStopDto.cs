using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto
{
    public class BusStopDto
    {
        public string Name { get; set; }
        public List<Bus> buses { get; set; }
    }
}
