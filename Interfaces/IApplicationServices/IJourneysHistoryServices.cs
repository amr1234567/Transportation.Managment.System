using Core.Dto.ServiceInput;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.IApplicationServices
{
    public interface IJourneysHistoryServices
    {
        void AddJourney(JourneyDto journeyDto);
        Task<List<JourneyHistory>> GetAllJourneys();
        Task<JourneyHistory> GetJourneyById(Guid id);
    }
}
