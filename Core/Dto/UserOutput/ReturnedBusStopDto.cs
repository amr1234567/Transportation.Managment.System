using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserOutput
{
    public class ReturnedBusStopDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<ReturnedBusStopDto> busStops { get; set; }
    }
}
