using Core.Constants;
using Core.Dto;
using Core.Dto.UserOutput;
using Core.Models;
using Interfaces.IApplicationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Roles.BusStopManager},{Roles.User},{Roles.Admin}")]
    public class JourneyController(IJourneysHistoryServices journeyService, IUpcomingJourneysServices UpcomingJourneysServices) : ControllerBase
    {
        private readonly IJourneysHistoryServices _journeysHistoryService = journeyService;
        private readonly IUpcomingJourneysServices _UpcomingJourneysServices = UpcomingJourneysServices;

        [AllowAnonymous]
        [HttpGet("All")]
        public async Task<ActionResult<ResponseModel<List<ReturnTimeTableDto>>>> GetAll()
        {
            var journeys = await _UpcomingJourneysServices.GetAllUpcomingJourneys();
            return Ok(new ResponseModel<List<ReturnedUpcomingJourneyDto>>
            {
                StatusCode = 200,
                Body = journeys.Select(j => new ReturnedUpcomingJourneyDto
                {
                    ArrivalTime = j.ArrivalTime,
                    BusId =
                }),
                Message = "Done"
            });
        }

        [HttpGet("All/{id}")]
        public async Task<ActionResult<ResponseModel<JourneyHistory>>> GetJourneyById(Guid id)
        {
            return Ok(new ResponseModel<JourneyHistory>
            {
                StatusCode = 200,
                Body = await _journeysHistoryService.GetJourneyById(id),
                Message = "Done"
            });
        }

        [HttpGet("Nearest/{destinationId}/{startBusStopId}")]
        public async Task<ActionResult<ResponseModel<JourneyHistory>>> GetNearestJourneyByDestination(string destinationId, string startBusStopId)
        {
            return Ok(new ResponseModel<UpcomingJourney>
            {
                StatusCode = 200,
                Body = await _UpcomingJourneysServices.GetNearestJourneyByDestination(destinationId, startBusStopId),
                Message = "Done"
            });
        }

        [HttpGet("AllByDestination/{destinationId}")]
        public async Task<ActionResult<ResponseModel<List<JourneyHistory>>>> GetAllJourneysByDestinationBusStopId(string destinationId)
        {
            return Ok(new ResponseModel<List<UpcomingJourney>>
            {
                StatusCode = 200,
                Body = await _UpcomingJourneysServices.GetAllJourneysByDestinationBusStopId(destinationId),
                Message = "Done"
            });
        }

        [HttpGet("AllByStart/{startBusStopId}")]
        public async Task<ActionResult<ResponseModel<List<JourneyHistory>>>> GetAllJourneysByStartBusStopId(string startBusStopId)
        {
            return Ok(new ResponseModel<List<UpcomingJourney>>
            {
                StatusCode = 200,
                Body = await _UpcomingJourneysServices.GetAllJourneysByStartBusStopId(startBusStopId),
                Message = "Done"
            });
        }
    }
}
