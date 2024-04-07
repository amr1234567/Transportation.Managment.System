using Core.Dto;
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
        Task<TimeTableDto> SetUpcomingJourneys(TimeTableDto time);
        void RemoveUpcomingJourneys();
        Task<List<ReturnTimeTableDto>> GetAllUpcomingJourneys();
        Task<UpcomingJourney> GetJourneyById(Guid id);
        Task<List<UpcomingJourney>> GetAllJourneysByDestinationBusStopId(string id);
        Task<List<UpcomingJourney>> GetAllJourneysByStartBusStopId(string id);
        Task<UpcomingJourney> GetNearestJourneyByDestination(string destinationId, string startBusStopId);
        Task SetArrivalTime(DateTime time, Guid id);
        Task SetLeavingTime(DateTime time, Guid id);
    }
}
