using Core.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Identity
{
    public class ApplicationUser : User
    {
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "{0} must contain value")]
        public string PhoneNumber { get; set; }

        public List<Ticket> Tickets { get; set; }
        public List<Journey> Journeys { get; set; }
    }
}
