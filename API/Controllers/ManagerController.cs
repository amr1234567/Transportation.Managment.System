using Core.Constants;
using Core.Dto.Identity;
using Core.Dto;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Interfaces.IApplicationServices;
using Core.Models;
using System.Security.Claims;
using Hangfire;
using Core.Dto.UserInput;
using Core.Dto.UserOutput;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.BusStopManager)]
    [ApiController]
    public class ManagerController(IBusServices busServices, ITicketServices ticketServices, IManagerServices managerServices, IJourneysHistoryServices journeyServices, IUpcomingJourneysServices timeTableService) : ControllerBase
    {
        private readonly IBusServices _busServices = busServices;
        private readonly ITicketServices _ticketServices = ticketServices;
        private readonly IManagerServices _managerServices = managerServices;
        private readonly IJourneysHistoryServices _journeyServices = journeyServices;
        private readonly IUpcomingJourneysServices _timeTableService = timeTableService;

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

        [HttpGet("AllJourneys")]
        public async Task<ActionResult<ResponseModel<List<JourneyHistory>>>> GetAllJourneys()
        {
            var journeys = await _journeyServices.GetAllJourneys();
            return Ok(new ResponseModel<List<JourneyHistory>>
            {
                Body = journeys,
                Message = "ALl Journeys",
                StatusCode = 200
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


        [HttpPost("Add-Journey")]
        public async Task<ActionResult<ResponseModel<bool>>> AddJourney([FromBody] TimeTableDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<Bus>
                {
                    StatusCode = 400,
                    Message = "Error"
                });
            var bus = await _busServices.GetBusById(model.BusId);
            if (bus.IsAvailable == false)
            {
                return Ok(new ResponseModel<bool>
                {
                    Body = false,
                    Message = "bus on another journey added",
                    StatusCode = 400
                });
            }
            await _timeTableService.SetUpcomingJourneys(model);

            TimeSpan duration = model.ArrivalTime - DateTime.UtcNow;
            BackgroundJob.Schedule(() => _timeTableService.RemoveUpcomingJourneys(), duration);

            return Ok(new ResponseModel<bool>
            {
                Body = true,
                Message = "Journey added",
                StatusCode = 200
            });
        }

        [HttpDelete("remove-Time-Table")]
        public ActionResult Remove()
        {
            _timeTableService.RemoveUpcomingJourneys();
            return Ok();
        }

        [HttpPost("CutTicket")]
        public async Task<ActionResult<ResponseModel<bool>>> CutTicket(TicketDto model)
        {

            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<bool>
                {
                    StatusCode = 400,
                    Message = "Input is invalid",
                    Body = false
                });
            var ticket = await _ticketServices.CutTicket(model, GetUserIdFromClaims());
            return Ok(new ResponseModel<bool>
            {
                Message = "Done Booking",
                StatusCode = 200,
                Body = true
            });
        }
        private string GetUserIdFromClaims()
        {
            return User.FindFirstValue("Id");
        }

    }
}
