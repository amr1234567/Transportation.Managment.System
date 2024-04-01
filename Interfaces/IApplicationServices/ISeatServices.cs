using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.IApplicationServices
{
    public interface ISeatServices
    {
        Task ReserveSeat(int id);
    }
}
