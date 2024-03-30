using Core.Dto;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices
{
    public class SeatServices : ISeatServices
    {
        private readonly ApplicationDbContext _context;

        public SeatServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddSeat(SeatDto seatDto)
        {
            // Implement here bitch
            //create new seat and add it to db
            throw new NotImplementedException();
        }

        public Task ReserveSeat(int id, SeatDto seatDto)
        {
            // Implement here bitch
            //edit on seat field "IsAvailable" to false
            throw new NotImplementedException();
        }
    }
}
