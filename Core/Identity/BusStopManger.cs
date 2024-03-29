using Microsoft.AspNetCore.Identity;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Identity
{
    public class BusStopManger:IdentityUser
    {
        public BusStop BusStop { get; set; }
    }
}
