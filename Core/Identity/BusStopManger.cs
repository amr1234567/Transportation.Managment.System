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

        [ForeignKey(nameof(BusStop))]
        public Guid? BusStopId { get; set; }
        public BusStop? BusStop { get; set; }
    }
}
