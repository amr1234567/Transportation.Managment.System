using Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.IServices
{
    public interface ISeatServices
    {
        Task AddSeat(SeatDto seatDto);
        Task ReserveSeat(int id, SeatDto seatDto);
    }
}
