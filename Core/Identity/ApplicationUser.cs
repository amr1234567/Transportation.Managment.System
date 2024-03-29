using Core.Models;
using Microsoft.AspNetCore.Identity;
using Model.Identity;

namespace Core.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<Journey> Journeys { get; set; }
    }
}
