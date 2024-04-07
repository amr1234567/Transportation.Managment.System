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
using Services.IdentityServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IAdminServices adminServices, IManagerServices managerServices, IUpcomingJourneysServices upcomingJourneysServices, IJourneysHistoryServices journeysHistoryServices) : ControllerBase
    {
        private readonly IAdminServices _adminServices = adminServices;
        private readonly IManagerServices _managerServices = managerServices;
        private readonly IUpcomingJourneysServices _upcomingJourneysServices = upcomingJourneysServices;
        private readonly IJourneysHistoryServices _journeysHistoryServices = journeysHistoryServices;

        //[NonAction]
        //[Authorize(Roles = Roles.Admin)]
        [HttpPost("SignUp")]
        public async Task<ActionResult<ResponseModel<string>>> SignUp(SignUpDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _adminServices.SignUp(model);
            if (response)
                return Ok(new ResponseModel<string>
                {
                    StatusCode = 200,
                    Message = "Every thing is good"
                });
            return BadRequest(new ResponseModel<string>
            {
                StatusCode = 400,
                Message = "Wrong Email Or Password"
            });
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult<ResponseModel<TokenModel>>> SignIn(LogInDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _adminServices.SignIn(model);
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

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("CreateManager")]
        public async Task<ActionResult<ResponseModel<string>>> CreateManager(SignUpDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _managerServices.SignUp(model);
            if (response)
                return Ok(new ResponseModel<string>
                {
                    StatusCode = 200,
                    Message = "Every thing is good"
                });
            return BadRequest(new ResponseModel<string>
            {
                StatusCode = 400,
                Message = "Wrong Email Or Password"
            });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("Add-BusStop-To-Another")]
        public async Task<ActionResult<ResponseModel<bool>>> AddBusStopToAnother(string Id, string BusStopId)
        {
            await _managerServices.enrollBusStop(Id, BusStopId);
            return Ok(new ResponseModel<bool>
            {
                StatusCode = 200,
                Body = true,
                Message = "BusStop Added Successfully"

            });
        }

        [HttpGet("AllJourneys")]
        public async Task<ActionResult<ResponseModel<List<ReturnTimeTableDto>>>> GetAllJourneysInDb()
        {
            var UpcomingJourneys = await _upcomingJourneysServices.GetAllUpcomingJourneys();
            var HistoryJourneys = await _journeysHistoryServices.GetAllJourneys();
            return Ok(new ResponseModel<List<ReturnTimeTableDto>>
            {
                StatusCode = 200,
                Body = UpcomingJourneys.Select(uj => new ReturnTimeTableDto
                {
                    ArrivalTime = uj.ArrivalTime,
                    DestinationName = uj.DestinationName,
                    LeavingTime = uj.LeavingTime,
                    NumberOfAvailableTickets = uj.NumberOfAvailableTickets,
                    StartBusStopName = uj.StartBusStopName,
                    TicketPrice = uj.TicketPrice
                })
                .Union(HistoryJourneys.Select(uj => new ReturnTimeTableDto
                {
                    ArrivalTime = uj.ArrivalTime,
                    LeavingTime = uj.LeavingTime
                }))
                .ToList(),
                Message = "All Journeys"
            });
        }
    }
}
