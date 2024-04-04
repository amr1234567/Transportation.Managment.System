using Core.Identity;
using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ReturnedTicketDto
    {
        public Guid SeatId { get; set; }
        public int SeatNumber { get; set; }

        public Guid BusId { get; set; }

        public Guid JourneyId { get; set; }

        public DateTime CreatedTime { get; set; }

        public string? UserName { get; set; }
        public double Price { get; set; }
        public bool IsFinished { get; set; }
    }
}
