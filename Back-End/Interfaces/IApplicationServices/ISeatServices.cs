using Core.Dto;
using Core.Dto.UserOutput;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.IApplicationServices
{
    public interface ISeatServices
    {
        Task<IEnumerable<SeatDto>> GetAllSeatsInBusByBusId(Guid BusId);
        Task ReserveSeat(Guid id);
    }
}
