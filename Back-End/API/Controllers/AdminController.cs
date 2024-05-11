using Core.Constants;
using Core.Dto.Identity;
using Core.Dto.UserInput;
using Core.Dto.UserOutput;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    [ApiController]
    public class AdminController(IBusServices busService, IAdminServices adminServices, IManagerServices managerServices, IJourneysHistoryServices journeysHistoryServices, ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IAdminServices _adminServices = adminServices;
        private readonly IManagerServices _managerServices = managerServices;
        private readonly IJourneysHistoryServices _journeysHistoryServices = journeysHistoryServices;
        private readonly IBusServices _busService = busService;

        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [NonAction]
        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<ActionResult<ResponseModel<string>>> SignUp(SignUpAsAdminDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var response = await _adminServices.SignUp(model);
                if (response)
                {
                    Log.Information($"Sign up Succeeded");
                    return Ok(new ResponseModel<string>
                    {
                        StatusCode = 200,
                        Message = "Every thing is good"
                    });
                }
                Log.Error($"Sign up Failed");
                return BadRequest(new ResponseModel<string>
                {
                    StatusCode = 400,
                    Message = "Wrong Email Or Password"
                });

            }
            catch (Exception ex)
            {
                Log.Error($"Sign up Failed");
                return BadRequest(new ResponseModel<string>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [ProducesResponseType(typeof(ResponseModel<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<ActionResult<ResponseModel<TokenModel>>> SignIn(LogInDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var response = await _adminServices.SignIn(model);
                if (response.StatusCode == 200)
                {
                    Log.Information($"Sign in Succeeded");
                    return Ok(new ResponseModel<TokenModel>
                    {
                        StatusCode = 200,
                        Message = "Every thing is good",
                        Body = response.TokenModel
                    });
                }
                Log.Error($"Sign in Failed");
                return BadRequest(new ResponseModel<string>
                {
                    StatusCode = 400,
                    Message = "Bad Input"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Sign in Failed ({ex.Message})");
                return BadRequest(new ResponseModel<TokenModel>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpPost("create-manager")]
        public async Task<ActionResult<ResponseModel<bool>>> CreateManager(SignUpAsManagerDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var response = await _managerServices.SignUp(model);
                if (response)
                {
                    Log.Information($"Manager Created");
                    return Ok(new ResponseModel<bool>
                    {
                        StatusCode = 200,
                        Message = "Every thing is good",
                        Body = true
                    });
                }
                Log.Error($"Manager Creation Failed");
                return BadRequest(new ResponseModel<bool>
                {
                    StatusCode = 400,
                    Message = "Bad"
                });

            }
            catch (Exception ex)
            {
                Log.Error($"Manager Creation Failed ({ex.Message})");
                return BadRequest(new ResponseModel<string>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpPost("enroll-bus-stop-to-bus-stop")]
        public async Task<ActionResult<ResponseModel<bool>>> EnrollBusStopToAnother(string StartBusStopId, string DestinationBusStopId)
        {
            try
            {
                await _managerServices.enrollBusStop(StartBusStopId, DestinationBusStopId);
                Log.Information($"Enrolled Process Succeeded");
                return Ok(new ResponseModel<bool>
                {
                    StatusCode = 200,
                    Body = true,
                    Message = "BusStop Added Successfully"

                });
            }
            catch (Exception ex)
            {
                Log.Error($"Enrolled Process Failed ({ex.Message})");
                return BadRequest(new ResponseModel<bool>
                {
                    StatusCode = 400,
                    Body = false,
                    Message = ex.Message
                });
            }
        }

        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedHistoryJourneyDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedHistoryJourneyDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet("get-all-history-journeys")]
        public async Task<ActionResult<ResponseModel<IEnumerable<ReturnedHistoryJourneyDto>>>> GetAllJourneysInDb()
        {
            try
            {
                var time = DateTime.UtcNow;
                var HistoryJourneys = await _journeysHistoryServices.GetAllJourneys();
                Log.Information($"Get All Journeys Succeeded({time} -> {DateTime.UtcNow})");
                return Ok(new ResponseModel<IEnumerable<ReturnedHistoryJourneyDto>>
                {
                    StatusCode = 200,
                    Body = HistoryJourneys.Select(hj => new ReturnedHistoryJourneyDto
                    {
                        ArrivalTime = hj.ArrivalTime,
                        BusId = hj.BusId,
                        LeavingTime = hj.LeavingTime,
                        NumberOfAvailableTickets = hj.Tickets.Count(),
                        DestinationName = hj.Destination.Name,
                        StartBusStopName = hj.StartBusStop.Name,
                        TicketPrice = hj.TicketPrice
                    }),
                    Message = "All Journeys"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Journeys Failed ({ex.Message})");
                return BadRequest(new ResponseModel<List<ReturnedHistoryJourneyDto>>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [ProducesResponseType(typeof(ResponseModel<IEnumerable<Bus>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<Bus>>), StatusCodes.Status400BadRequest)]
        [HttpGet("get-all-buses")]
        public async Task<ActionResult<ResponseModel<IEnumerable<Bus>>>> GetAllBuses()
        {
            try
            {
                var buses = await _busService.GetAllBuses();
                var model = new ResponseModel<IEnumerable<Bus>>
                {

                    StatusCode = 200,
                    Message = "Done",
                    Body = buses

                };
                Log.Information("Get All Buses Success");
                return model;
            }
            catch (Exception ex)
            {
                var model = new ResponseModel<IEnumerable<Bus>>
                {
                    StatusCode = 404,
                    Message = ex.Message,
                    Body = new List<Bus>()
                };
                Log.Error($"Get All Buses Error:{ex.Message}");
                return model;
            }

        }

    }
}
