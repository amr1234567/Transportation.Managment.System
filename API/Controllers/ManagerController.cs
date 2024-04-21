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
using Serilog;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.BusStopManager)]
    [ApiController]
    public class ManagerController(IBusServices busServices, ITicketServices ticketServices, IManagerServices managerServices, IUpcomingJourneysServices UpcomingJourneysServices) : ControllerBase
    {
        private readonly IBusServices _busServices = busServices;
        private readonly ITicketServices _ticketServices = ticketServices;
        private readonly IManagerServices _managerServices = managerServices;
        private readonly IUpcomingJourneysServices _UpcomingJourneysServices = UpcomingJourneysServices;

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<ActionResult<ResponseModel<TokenModel>>> SignIn(LogInDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var response = await _managerServices.SignIn(model);

                Log.Information($"Sign In Succeeded");
                return Ok(new ResponseModel<TokenModel>
                {
                    StatusCode = 200,
                    Message = "Every thing is good",
                    Body = response.TokenModel
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Can't Sign In ({ex.Message})");
                return BadRequest(new ResponseModel<TokenModel>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("get-all-history-journeys")]
        public async Task<ActionResult<ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>>> GetAllJourneys()
        {
            try
            {
                var journeys = await _UpcomingJourneysServices.GetAllJourneysByStartBusStopId(GetManagerIdFromClaims());
                Log.Information($"GetAllJourneys Succeeded");
                return Ok(new ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>
                {
                    Body = journeys,
                    Message = "ALl Journeys",
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                Log.Error($"GetAllJourneys Failed ({ex.Message})");
                return BadRequest(new ResponseModel<IEnumerable<JourneyHistory>>
                {
                    Message = ex.Message,
                    StatusCode = 400
                });
            }
        }

        [HttpPost("add-bus")]
        public async Task<ActionResult<ResponseModel<bool>>> AddBus([FromBody] BusDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<Bus>
                    {
                        StatusCode = 400,
                        Message = "Error"
                    });
                await _busServices.AddBus(model);
                Log.Information($"AddBus Succeeded");
                return Ok(new ResponseModel<Bus>
                {
                    StatusCode = 200,
                    Message = "Bus Added"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"AddBus Failed ({ex.Message})");
                return BadRequest(new ResponseModel<Bus>
                {
                    StatusCode = 200,
                    Message = ex.Message
                });
            }
        }


        [HttpPost("add-journey")]
        public async Task<ActionResult<ResponseModel<bool>>> AddJourney([FromBody] UpcomingJourneyDto model)
        {
            try
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
                    Log.Error($"AddBus Failed (bus not available)");
                    return BadRequest(new ResponseModel<bool>
                    {
                        Body = false,
                        Message = "bus on another journey added",
                        StatusCode = 400
                    });
                }
                await _UpcomingJourneysServices.AddUpcomingJourney(model);

                TimeSpan duration = model.ArrivalTime - DateTime.UtcNow;
                BackgroundJob.Schedule(() => _UpcomingJourneysServices.TurnUpcomingJourneysIntoHistoryJourneys(), duration);

                Log.Information($"AddJourney Succeeded");
                return Ok(new ResponseModel<bool>
                {
                    Body = true,
                    Message = "Journey added",
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                Log.Error($"AddJourney Failed ({ex.Message})");
                return BadRequest(new ResponseModel<Bus>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [NonAction]
        [HttpDelete("turn-upcoming-journeys-into-history-journeys")]
        public ActionResult TurnUpcomingJourneysIntoHistoryJourneys()
        {
            try
            {
                _UpcomingJourneysServices.TurnUpcomingJourneysIntoHistoryJourneys();
                Log.Information($"Empty the Upcoming journeys Succeeded");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error($"Empty the Upcoming journeys Failed ({ex.Message})");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("cut-ticket")]
        public async Task<ActionResult<ResponseModel<bool>>> CutTicket(TicketDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<bool>
                    {
                        StatusCode = 400,
                        Message = "Input is invalid",
                        Body = false
                    });
                var ticket = await _ticketServices.CutTicket(model, GetManagerIdFromClaims());
                Log.Information($"CutTicket Succeeded");
                return Ok(new ResponseModel<bool>
                {
                    Message = "Done Booking",
                    StatusCode = 200,
                    Body = true
                });
            }
            catch (Exception ex)
            {
                Log.Error($"CutTicket Failed");
                return BadRequest(new ResponseModel<bool>
                {
                    Message = ex.Message,
                    StatusCode = 400,
                    Body = false
                });
            }
        }

        private string GetManagerIdFromClaims()
        {
            return User.FindFirstValue("Id");
        }

    }
}
