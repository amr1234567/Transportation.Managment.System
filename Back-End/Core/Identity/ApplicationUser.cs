using Core.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Identity
{
    public class ApplicationUser : User
    {

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "{0} must contain a value")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+20(10|15|11|12)\d{8}")]
        [StringLength(13)]
        public string PhoneNumber { get; set; }

        //public List<Ticket>? Tickets { get; set; }
        //public List<JourneyHistory>? Journeys { get; set; }
    }
}
