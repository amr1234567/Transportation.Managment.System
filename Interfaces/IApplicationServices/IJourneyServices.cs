using Core.Dto;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.IApplicationServices
{
    public interface IJourneyServices
    {
        Task AddJourney(JourneyDto journeyDto, Guid DestinationId, Guid StartNusStopId, Guid BusId);
        Task SetArrivalTime(DateTime time, Guid id);
        Task SetLeavingTime(DateTime time, Guid id);
        Task<List<Journey>> GetAllJourneys();
        Task<Journey> GetNearestJourneyByDestination(Guid destinationId, Guid startBusStopId);
        Task<Journey> GetJourneyById(Guid id);
        Task<List<Journey>> GetAllJourneysByDestinationBusStopId(Guid id);
        Task<List<Journey>> GetAllJourneysByStartBusStopId(Guid id);
    }
}
