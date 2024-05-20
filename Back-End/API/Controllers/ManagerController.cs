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
    [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status401Unauthorized)]
    [ApiController]
    public class ManagerController(IBusServices busServices, ITicketServices ticketServices, IManagerServices managerServices, IUpcomingJourneysServices UpcomingJourneysServices) : ControllerBase
    {
        private readonly IBusServices _busServices = busServices;
        private readonly ITicketServices _ticketServices = ticketServices;
        private readonly IManagerServices _managerServices = managerServices;
        private readonly IUpcomingJourneysServices _UpcomingJourneysServices = UpcomingJourneysServices;

        [ProducesResponseType(typeof(ResponseModel<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<ActionResult> SignIn(LogInDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var response = await _managerServices.SignIn(model);

                Log.Information($"Sign In Succeeded");
                return Ok(response);
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

        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedUpcomingJourneyDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet("get-all-upcoming-journeys")]
        public async Task<ActionResult> GetAllJourneys()
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

        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpPost("add-bus")]
        public async Task<ActionResult> AddBus([FromBody] BusDto model)
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


        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpPost("add-journey")]
        public async Task<ActionResult> AddJourney([FromBody] UpcomingJourneyDto model)
        {
            try
            {
                if (!Guid.TryParse(model.BusId, out Guid BusId))
                    ModelState.AddModelError("busId", "Bus Id Must be a valid Guid");
                if (!Guid.TryParse(model.DestinationId, out _))
                    ModelState.AddModelError("destinationId", "Destination Id Must be a valid Guid");

                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<IEnumerable<ErrorModelState>>
                    {
                        StatusCode = 400,
                        Message = "Error",
                        Body = ModelState.Keys.Select(key => new ErrorModelState(key, ModelState[key].Errors.Select(x => x.ErrorMessage).ToList()))
                    });

                model.StartBusStopId = GetManagerIdFromClaims();
                var bus = await _busServices.GetBusById(BusId);
                if (bus is null || bus.IsAvailable == false)
                {
                    Log.Error($"AddBus Failed (bus not available)");
                    return BadRequest(new ResponseModel<bool>
                    {
                        Body = false,
                        Message = "bus on another journey added",
                        StatusCode = 400
                    });
                }
                model.StartBusStopId = GetManagerIdFromClaims();
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
                return BadRequest(new ResponseModel<IEnumerable<ErrorModelState>?>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Body = null
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

        [ProducesResponseType(typeof(ResponseModel<ReturnedTicketDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpPost("cut-ticket")]
        public async Task<ActionResult> CutTicket(TicketDto model)
        {
            try
            {
                if (!Guid.TryParse(model.JourneyId.ToString(), out _))
                    ModelState.AddModelError("JourneyId", "Journey Id Must be a valid Guid");
                if (!Guid.TryParse(model.SeatId.ToString(), out _))
                    ModelState.AddModelError("SeatId", "Seat Id Must be a valid Guid");

                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<IEnumerable<ErrorModelState>>
                    {
                        StatusCode = 400,
                        Message = "Input is invalid",
                        Body = ModelState.Keys.Select(key => new ErrorModelState(key, ModelState[key].Errors.Select(x => x.ErrorMessage).ToList()))
                    });
                var ticket = await _ticketServices.CutTicket(model, GetManagerIdFromClaims());
                Log.Information($"CutTicket Succeeded");
                return ticket.StatusCode == 200 ? Ok(ticket) : BadRequest(ticket);
            }
            catch (Exception ex)
            {
                Log.Error($"CutTicket Failed");
                return BadRequest(new ResponseModel<ReturnedTicketDto>
                {
                    Message = ex.Message,
                    StatusCode = 400
                });
            }
        }

        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedBusStopDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedBusStopDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet("get-all-related-by-manager")]
        public async Task<ActionResult> GetAllRelatedBusStops()
        {
            try
            {
                var records = await _managerServices.GetAllDestinationBusStops(GetManagerIdFromClaims());
                Log.Information("Get All BusStops succeeded");
                return Ok(new ResponseModel<IEnumerable<ReturnedBusStopDto>>()
                {
                    Body = records,
                    Message = "Done",
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All related BusStops Failed :{ex.Message}");
                return Ok(new ResponseModel<IEnumerable<BusStopDto>>()
                {
                    Body = new List<BusStopDto>(),
                    Message = $"Get All related BusStops Failed :{ex.Message}",
                    StatusCode = 400
                });
            }

        }


        private string GetManagerIdFromClaims()
        {
            return User.FindFirstValue("Id");
        }
    }
}
