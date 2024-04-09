using Core.Constants;
using Core.Dto;
using Core.Dto.UserOutput;
using Core.Models;
using Interfaces.IApplicationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Roles.BusStopManager},{Roles.User},{Roles.Admin}")]
    public class JourneyController(IUpcomingJourneysServices UpcomingJourneysServices) : ControllerBase
    {
        private readonly IUpcomingJourneysServices _UpcomingJourneysServices = UpcomingJourneysServices;

        [AllowAnonymous]
        [HttpGet("All")]
        public async Task<ActionResult<ResponseModel<List<ReturnedUpcomingJourneyDto>>>> GetAll()
        {
            try
            {
                var journeys = await _UpcomingJourneysServices.GetAllUpcomingJourneys();
                Log.Information($"Get All Journeys Succeeded");
                return Ok(new ResponseModel<List<ReturnedUpcomingJourneyDto>>
                {
                    StatusCode = 200,
                    Body = journeys,
                    Message = "Done"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Journeys failed {ex.Message}");
                return new ResponseModel<List<ReturnedUpcomingJourneyDto>>
                {
                    Message = ex.Message,
                    StatusCode = 400
                };
            }

        }

        [HttpGet("All/{id}")]
        public async Task<ActionResult<ResponseModel<ReturnedUpcomingJourneyDto>>> GetJourneyById(Guid id)
        {
            try
            {
                var journey = await _UpcomingJourneysServices.GetJourneyById(id);
                Log.Information($"Get Journey By Id Succeeded");
                return Ok(new ResponseModel<ReturnedUpcomingJourneyDto>
                {
                    StatusCode = 200,
                    Body = journey,
                    Message = "Done"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get Journey By Id failed {ex.Message}");
                return BadRequest(new ResponseModel<ReturnedUpcomingJourneyDto>
                {
                    StatusCode = 400,
                    Message = $"Get Journey Failed {ex.Message}"
                });
            }

        }

        [HttpGet("Nearest/{destinationId}/{startBusStopId}")]
        public async Task<ActionResult<ResponseModel<ReturnedUpcomingJourneyDto>>> GetNearestJourneyByDestination(string destinationId, string startBusStopId)
        {
            try
            {
                var journey = await _UpcomingJourneysServices.GetNearestJourneyByDestination(destinationId, startBusStopId);
                Log.Information($"Get Nearest Journey By Destination Succeeded");
                return Ok(new ResponseModel<ReturnedUpcomingJourneyDto>
                {
                    StatusCode = 200,
                    Body = journey,
                    Message = "Done"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get Nearest Journey By Destination failed {ex.Message}");
                return BadRequest(new ResponseModel<ReturnedUpcomingJourneyDto>
                {
                    StatusCode = 400,
                    Message = $"Get Nearest Journey By Destination failed  {ex.Message}"
                });
            }

        }

        [HttpGet("AllByDestination/{destinationId}")]
        public async Task<ActionResult<ResponseModel<List<ReturnedUpcomingJourneyDto>>>> GetAllJourneysByDestinationBusStopId(string destinationId)
        {
            try
            {
                var journeys = await _UpcomingJourneysServices.GetAllJourneysByDestinationBusStopId(destinationId);
                Log.Information($"Get All Journeys By Destination Bus Stop Id Succeeded");
                return Ok(new ResponseModel<List<ReturnedUpcomingJourneyDto>>
                {
                    StatusCode = 200,
                    Body = journeys,
                    Message = "Done"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Journeys By Destination Bus Stop Id failed {ex.Message}");
                return BadRequest(new ResponseModel<List<ReturnedUpcomingJourneyDto>>
                {
                    StatusCode = 400,
                    Message = $"Get All Journeys By Destination Bus Stop Id failed {ex.Message}"
                });
            }

        }

        [HttpGet("AllByStart/{startBusStopId}")]
        public async Task<ActionResult<ResponseModel<List<ReturnedUpcomingJourneyDto>>>> GetAllJourneysByStartBusStopId(string startBusStopId)
        {
            try
            {
                var Journeys = await _UpcomingJourneysServices.GetAllJourneysByStartBusStopId(startBusStopId);
                Log.Information($"Get All Journeys By Start Bus Stop Id Succeeded");
                return Ok(new ResponseModel<List<ReturnedUpcomingJourneyDto>>
                {
                    StatusCode = 200,
                    Body = Journeys,
                    Message = "Done"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Journeys By Start Bus Stop Id failed {ex.Message}");
                return BadRequest(new ResponseModel<List<ReturnedUpcomingJourneyDto>>
                {
                    StatusCode = 400,
                    Message = $"Get All Journeys By Start Bus Stop Id failed {ex.Message}"
                });
            }
        }
    }
}
