using Core.Constants;
using Core.Dto;
using Core.Dto.Identity;
using Core.Dto.UserInput;
using Core.Dto.UserOutput;
using Interfaces.IApplicationServices;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Services.IdentityServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    [ApiController]
    public class AdminController(IAdminServices adminServices, IManagerServices managerServices, IJourneysHistoryServices journeysHistoryServices) : ControllerBase
    {
        private readonly IAdminServices _adminServices = adminServices;
        private readonly IManagerServices _managerServices = managerServices;
        private readonly IJourneysHistoryServices _journeysHistoryServices = journeysHistoryServices;

        [NonAction]
        //[AllowAnonymous]
        [HttpPost("SignUp")]
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

        [AllowAnonymous]
        [HttpPost("SignIn")]
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
                    Message = "Bad"
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

        [HttpPost("CreateManager")]
        public async Task<ActionResult<ResponseModel<string>>> CreateManager(SignUpAsManagerDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var response = await _managerServices.SignUp(model);
                if (response)
                {
                    Log.Information($"Manager Created");
                    return Ok(new ResponseModel<string>
                    {
                        StatusCode = 200,
                        Message = "Every thing is good"
                    });
                }
                Log.Error($"Manager Creation Failed");
                return BadRequest(new ResponseModel<string>
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

        [HttpPost("Add-BusStop-To-Another")]
        public async Task<ActionResult<ResponseModel<bool>>> AddBusStopToAnother(string Id, string BusStopId)
        {
            try
            {
                await _managerServices.enrollBusStop(Id, BusStopId);
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

        [HttpGet("AllJourneys")]
        public async Task<ActionResult<ResponseModel<List<ReturnTimeTableDto>>>> GetAllJourneysInDb()
        {
            try
            {
                var HistoryJourneys = await _journeysHistoryServices.GetAllJourneys();
                Log.Information($"Get All Journeys Succeeded");
                return Ok(new ResponseModel<List<ReturnTimeTableDto>>
                {
                    StatusCode = 200,
                    Body = HistoryJourneys.Select(hj => new ReturnTimeTableDto
                    {
                        ArrivalTime = hj.ArrivalTime,
                        BusId = hj.BusId,
                        LeavingTime = hj.LeavingTime,
                        NumberOfAvailableTickets = hj.Tickets.Count,
                        DestinationName = hj.Destination.Name,
                        StartBusStopName = hj.StartBusStop.Name,
                        TicketPrice = hj.TicketPrice
                    }).ToList(),
                    Message = "All Journeys"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Journeys Failed ({ex.Message})");
                return BadRequest(new ResponseModel<List<ReturnTimeTableDto>>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }
    }
}
