﻿using Core.Constants;
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
    public class UpcomingJourneyController(IUpcomingJourneysServices UpcomingJourneysServices) : ControllerBase
    {
        private readonly IUpcomingJourneysServices _UpcomingJourneysServices = UpcomingJourneysServices;


        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpGet("get-all-upcoming-journeys")]
        public async Task<ActionResult> GetAllUpcomingJourneys()
        {
            try
            {
                var journeys = await _UpcomingJourneysServices.GetAllUpcomingJourneys();
                Log.Information($"Get All Journeys Succeeded");
                return Ok(new ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>
                {
                    StatusCode = 200,
                    Body = journeys,
                    Message = "Done"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Journeys failed {ex.Message}");
                return BadRequest(new ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>
                {
                    Message = ex.Message,
                    StatusCode = 400
                });
            }

        }

        [ProducesResponseType(typeof(ResponseModel<ReturnedUpcomingJourneyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpGet("get-upcoming-journey/{UpcomingJourneyId}")]
        public async Task<ActionResult> GetJourneyById(Guid UpcomingJourneyId)
        {
            try
            {
                var journey = await _UpcomingJourneysServices.GetJourneyById(UpcomingJourneyId);
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
                return BadRequest(new ResponseModel<IEnumerable<ErrorModelState>>
                {
                    StatusCode = 400,
                    Message = $"Get Journey Failed {ex.Message}"
                });
            }

        }

        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpGet("get-all-Nearest-upcoming-journey/{destinationId}/{startBusStopId}")]
        public async Task<ActionResult> GetNearestJourneyByDestination(string destinationId, string startBusStopId)
        {
            try
            {
                var journey = await _UpcomingJourneysServices.GetNearestJourneysByDestination(destinationId, startBusStopId);
                Log.Information($"Get Nearest Journey By Destination Succeeded");
                return Ok(new ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>
                {
                    StatusCode = 200,
                    Body = journey,
                    Message = "Done"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get Nearest Journey By Destination failed {ex.Message}");
                return BadRequest(new ResponseModel<IEnumerable<ErrorModelState>>
                {
                    StatusCode = 400,
                    Message = $"Get Nearest Journey By Destination failed  {ex.Message}"
                });
            }

        }

        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpGet("get-all-by-destination/{destinationId}")]
        public async Task<ActionResult> GetAllJourneysByDestinationBusStopId(string destinationId)
        {
            try
            {
                var journeys = await _UpcomingJourneysServices.GetAllJourneysByDestinationBusStopId(destinationId);
                Log.Information($"Get All Journeys By Destination Bus Stop Id Succeeded");
                return Ok(new ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>
                {
                    StatusCode = 200,
                    Body = journeys,
                    Message = "Done"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Journeys By Destination Bus Stop Id failed {ex.Message}");
                return BadRequest(new ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>
                {
                    StatusCode = 400,
                    Message = $"Get All Journeys By Destination Bus Stop Id failed {ex.Message}"
                });
            }

        }

        [HttpGet("get-all-by-start/{startBusStopId}")]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetAllJourneysByStartBusStopId(string startBusStopId)
        {
            try
            {
                var Journeys = await _UpcomingJourneysServices.GetAllJourneysByStartBusStopId(startBusStopId);
                Log.Information($"Get All Journeys By Start Bus Stop Id Succeeded");
                return Ok(new ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>
                {
                    StatusCode = 200,
                    Body = Journeys,
                    Message = "Done"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Journeys By Start Bus Stop Id failed {ex.Message}");
                return BadRequest(new ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>
                {
                    StatusCode = 400,
                    Message = $"Get All Journeys By Start Bus Stop Id failed {ex.Message}"
                });
            }
        }
    }
}
