using Core.Constants;
using Core.Dto;
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
    public class JourneyController(IJourneyServices journeyService) : ControllerBase
    {
        private readonly IJourneyServices _journeyService = journeyService;

        [HttpGet("All")]
        public async Task<ActionResult<ResponseModel<List<Journey>>>> GetAll()
        {
            return Ok(new ResponseModel<List<Journey>>
            {
                StatusCode = 200,
                Body = await _journeyService.GetAllJourneys(),
                Message = "Done"
            });
        }

        [HttpGet("All/{id}")]
        public async Task<ActionResult<ResponseModel<Journey>>> GetJourneyById(Guid id)
        {
            return Ok(new ResponseModel<Journey>
            {
                StatusCode = 200,
                Body = await _journeyService.GetJourneyById(id),
                Message = "Done"
            });
        }

        [HttpGet("Nearest/{destinationId}/{startBusStopId}")]
        public async Task<ActionResult<ResponseModel<Journey>>> GetNearestJourneyByDestination(Guid destinationId, Guid startBusStopId)
        {
            return Ok(new ResponseModel<Journey>
            {
                StatusCode = 200,
                Body = await _journeyService.GetNearestJourneyByDestination(destinationId, startBusStopId),
                Message = "Done"
            });
        }

        [HttpGet("AllByDestination/{destinationId}")]
        public async Task<ActionResult<ResponseModel<List<Journey>>>> GetAllJourneysByDestinationBusStopId(Guid destinationId)
        {
            return Ok(new ResponseModel<List<Journey>>
            {
                StatusCode = 200,
                Body = await _journeyService.GetAllJourneysByDestinationBusStopId(destinationId),
                Message = "Done"
            });
        }

        [HttpGet("AllByStart/{startBusStopId}")]
        public async Task<ActionResult<ResponseModel<List<Journey>>>> GetAllJourneysByStartBusStopId(Guid startBusStopId)
        {
            return Ok(new ResponseModel<List<Journey>>
            {
                StatusCode = 200,
                Body = await _journeyService.GetAllJourneysByStartBusStopId(startBusStopId),
                Message = "Done"
            });
        }
    }
}
