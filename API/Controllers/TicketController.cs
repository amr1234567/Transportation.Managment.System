using Core.Constants;
using Core.Dto.UserOutput;
using Core.Helpers.Functions;
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
    public class TicketController(ITicketServices ticketServices) : ControllerBase
    {
        private readonly ITicketServices _ticketServices = ticketServices;

        [HttpGet("All")]
        public async Task<ActionResult<ResponseModel<List<ReturnedTicketDto>>>> GetAllTickets()
        {
            try
            {
                var time = DateTime.UtcNow;
                var tikets = await _ticketServices.GetAllTickets();
                Log.Information($"Get All Tickets Done Successfully ({time} -> {DateTime.UtcNow})");
                return Ok(new ResponseModel<List<ReturnedTicketDto>>
                {
                    StatusCode = 200,
                    Body = tikets,
                    Message = "Done"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Tickets Failed ({ex.Message})");
                return BadRequest(new ResponseModel<List<ReturnedTicketDto>>
                {
                    StatusCode = 400,
                    Message = "Failed " + ex.Message
                });
            }
        }

        [HttpGet("AllByJourney/{id}")]
        public async Task<ActionResult<ResponseModel<List<ReturnedTicketDto>>>> GetAllTicketsByJourneyId(Guid id)
        {
            try
            {
                var tikets = await _ticketServices.GetAllTicketsByJourneyId(id);
                Log.Information($"Get All Tickets By Journey Id Done Successfully");
                return Ok(new ResponseModel<List<ReturnedTicketDto>>
                {
                    StatusCode = 200,
                    Body = tikets,
                    Message = "Done"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Tickets By Journey Id Failed ({ex.Message})");
                return BadRequest(new ResponseModel<List<ReturnedTicketDto>>
                {
                    StatusCode = 400,
                    Message = "Failed " + ex.Message
                });
            }
        }

        [HttpGet("AllByReservedTime")]
        public async Task<ActionResult<ResponseModel<List<ReturnedTicketDto>>>> GetAllTicketsByReservedTime([FromRoute] DateTime time)
        {
            try
            {
                var tikets = await _ticketServices.GetTicketsByReservedTime(time);
                Log.Information($"Get All Tickets By Reserved Time Done Successfully");

                return Ok(new ResponseModel<List<ReturnedTicketDto>>
                {
                    StatusCode = 200,
                    Body = tikets,
                    Message = "Done"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Tickets By Reserved Time Failed ({ex.Message})");
                return BadRequest(new ResponseModel<List<ReturnedTicketDto>>
                {
                    StatusCode = 400,
                    Message = "Failed " + ex.Message
                });
            }
        }

        [HttpGet("ticket/{id}")]
        public async Task<ActionResult<ResponseModel<ReturnedTicketDto>>> GetTicket(Guid id)
        {
            try
            {
                var ticket = await _ticketServices.GetTicketById(id);
                Log.Information($"Get Ticket By Id Done Successfully");
                return Ok(new ResponseModel<ReturnedTicketDto>
                {
                    StatusCode = 200,
                    Message = "Done",
                    Body = ticket
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Tickets By Reserved Time Failed ({ex.Message})");
                return BadRequest(new ResponseModel<List<ReturnedTicketDto>>
                {
                    StatusCode = 400,
                    Message = "Failed " + ex.Message
                });
            }
        }
    }
}
