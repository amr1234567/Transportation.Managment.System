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
    public interface IUpcomingJourneysServices
    {
        Task<UpcomingJourneyDto> AddUpcomingJourney(UpcomingJourneyDto time);
        void TurnUpcomingJourneysIntoHistoryJourneys();
        Task<IEnumerable<ReturnedUpcomingJourneyDto>> GetAllUpcomingJourneys();
        Task<ReturnedUpcomingJourneyDto> GetJourneyById(Guid id);
        Task<IEnumerable<ReturnedUpcomingJourneyDto>> GetAllJourneysByDestinationBusStopId(string id);
        Task<IEnumerable<ReturnedUpcomingJourneyDto>> GetAllJourneysByStartBusStopId(string id);
        Task<IEnumerable<ReturnedUpcomingJourneyDto>> GetNearestJourneysByDestination(string destinationId, string startBusStopId);
        Task SetArrivalTime(DateTime time, Guid id);
        Task SetLeavingTime(DateTime time, Guid id);
    }
}
