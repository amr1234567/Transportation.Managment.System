using Microsoft.AspNetCore.Identity;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Identity
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Ticket> tickets { get; set; }
        public List<Journey> journeys { get; set; }
    }
}
