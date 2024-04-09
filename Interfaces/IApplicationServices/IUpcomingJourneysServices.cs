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
        Task<TimeTableDto> SetUpcomingJourneys(TimeTableDto time);
        void RemoveUpcomingJourneys();
        Task<List<ReturnedUpcomingJourneyDto>> GetAllUpcomingJourneys();
        Task<ReturnedUpcomingJourneyDto> GetJourneyById(Guid id);
        Task<List<ReturnedUpcomingJourneyDto>> GetAllJourneysByDestinationBusStopId(string id);
        Task<List<ReturnedUpcomingJourneyDto>> GetAllJourneysByStartBusStopId(string id);
        Task<ReturnedUpcomingJourneyDto> GetNearestJourneyByDestination(string destinationId, string startBusStopId);
        Task SetArrivalTime(DateTime time, Guid id);
        Task SetLeavingTime(DateTime time, Guid id);
    }
}
