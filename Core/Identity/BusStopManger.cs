using Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity
{
    public class BusStopManger : User
    {

        public List<BusStopManger>? BusStopsRelations { get; set; }
        public List<BusStopManger>? BusStops { get; set; }

        public List<Bus> Buses { get; set; }
    }
}
