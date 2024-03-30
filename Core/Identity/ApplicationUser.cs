using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.Identity
{
    public class ApplicationUser : User
    {
        public List<Ticket> Tickets { get; set; }
        public List<Journey> Journeys { get; set; }
    }
}
