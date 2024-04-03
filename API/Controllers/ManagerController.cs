using Core.Constants;
using Core.Dto.Identity;
using Core.Dto;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Interfaces.IApplicationServices;
using Core.Models;
using Core.Helpers.Functions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.BusStopManager)]
    [ApiController]
    public class ManagerController(IBusServices busServices, ITicketServices ticketServices, IManagerServices managerServices, IJourneyServices journeyServices) : ControllerBase
    {
        private readonly IBusServices _busServices = busServices;
        private readonly ITicketServices _ticketServices = ticketServices;
        private readonly IManagerServices _managerServices = managerServices;
        private readonly IJourneyServices _journeyServices = journeyServices;

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<ActionResult<ResponseModel<TokenModel>>> SignIn(LogInDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _managerServices.SignIn(model);
            if (response.StatusCode == 200)
                return Ok(new ResponseModel<TokenModel>
                {
                    StatusCode = 200,
                    Message = "Every thing is good",
                    Body = response.TokenModel
                });
            return BadRequest(new ResponseModel<TokenModel>
            {
                StatusCode = response.StatusCode,
                Message = "Wrong Email Or Password",
                Body = response.TokenModel
            });
        }

        [HttpPost("AddBus")]
        public async Task<ActionResult<ResponseModel<bool>>> AddBus([FromBody] BusDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<Bus>
                {
                    StatusCode = 400,
                    Message = "Error"
                });
            await _busServices.AddBus(model);
            return Ok(new ResponseModel<Bus>
            {
                StatusCode = 200,
                Message = "Bus Added"
            });
        }

        [HttpPost("AddJourney")]
        public async Task<ActionResult<ResponseModel<bool>>> AddJourney([FromBody] JourneyDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<bool>
                {
                    StatusCode = 400,
                    Message = "Error"
                });
            await _journeyServices.AddJourney(model);
            return Ok(new ResponseModel<bool>
            {
                StatusCode = 200,
                Message = "Journey Added"
            });
        }

        [HttpPost("SetArrivalTime/{id}")]
        public async Task<ActionResult<ResponseModel<bool>>> SetArrivalTime([FromBody] DateTime time, [FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<bool>
                {
                    StatusCode = 400,
                    Message = "Error"
                });
            await _journeyServices.SetArrivalTime(time, id);
            return Ok(new ResponseModel<bool>
            {
                StatusCode = 200,
                Message = "Journey Edited"
            });
        }

        [HttpPost("SetLeavingTime/{id}")]
        public async Task<ActionResult<ResponseModel<bool>>> SetLeavingTime([FromBody] DateTime time, [FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<bool>
                {
                    StatusCode = 400,
                    Message = "Error"
                });
            await _journeyServices.SetLeavingTime(time, id);
            return Ok(new ResponseModel<bool>
            {
                StatusCode = 200,
                Message = "Journey Edited"
            });
        }

        [HttpPost("CutTicket")]
        public async Task<ActionResult<ResponseModel<List<ReturnedTicketDto>>>> CutTicket(TicketDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<List<ReturnedTicketDto>>
                {
                    StatusCode = 400,
                    Message = "Input is invalid"
                });
            var ticket = await _ticketServices.CutTicket(model);
            return Ok(new ResponseModel<ReturnedTicketDto>
            {
                Message = "Done Booking",
                StatusCode = 200,
                Body = ticket.ConvertToDto()
            });
        }

    }
}
