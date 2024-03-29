using Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity
{
    public class BusStopManger : IdentityUser
    {
        public BusStop BusStop { get; set; }
    }
}
