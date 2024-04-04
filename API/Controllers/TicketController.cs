using Core.Constants;
using Core.Dto;
using Core.Helpers.Functions;
using Interfaces.IApplicationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var tikets = await _ticketServices.GetAllTickets();
            return Ok(new ResponseModel<List<ReturnedTicketDto>>
            {
                StatusCode = 200,
                Body = tikets.Convert(),
                Message = "Done"
            });
        }

        [HttpGet("AllByJourney/{id}")]
        public async Task<ActionResult<ResponseModel<List<ReturnedTicketDto>>>> GetAllTicketsByJourneyId(Guid id)
        {
            var tikets = await _ticketServices.GetAllTicketsByJourneyId(id);
            return Ok(new ResponseModel<List<ReturnedTicketDto>>
            {
                StatusCode = 200,
                Body = tikets.Convert(),
                Message = "Done"
            });
        }

        [HttpGet("AllByReservedTime")]
        public async Task<ActionResult<ResponseModel<List<ReturnedTicketDto>>>> GetAllTicketsByReservedTime([FromBody] DateTime time)
        {
            var tikets = await _ticketServices.GetTicketsByReservedTime(time);
            return Ok(new ResponseModel<List<ReturnedTicketDto>>
            {
                StatusCode = 200,
                Body = tikets.Convert(),
                Message = "Done"
            });
        }

        [HttpGet("ticket/{id}")]
        public async Task<ActionResult<ResponseModel<ReturnedTicketDto>>> GetTicket(Guid id)
        {
            var ticket = await _ticketServices.GetTicketById(id);
            return Ok(new ResponseModel<ReturnedTicketDto>
            {
                StatusCode = 200,
                Message = "Done",
                Body = ticket.ConvertToDto()
            });
        }
    }
}
