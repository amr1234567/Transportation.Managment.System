using Core.Constants;
using Core.Dto;
using Core.Dto.Identity;
using Core.Helpers.Functions;
using Interfaces.IApplicationServices;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserServices userServices, ITicketServices ticketServices) : ControllerBase
    {
        private readonly IUserServices _userServices = userServices;
        private readonly ITicketServices _ticketServices = ticketServices;

        [HttpPost("SignUp")]
        public async Task<ActionResult<ResponseModel<string>>> SignUp([FromBody] SignUpDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _userServices.SignUp(model);
            if (string.IsNullOrEmpty(response))
                return BadRequest(new ResponseModel<string>
                {
                    StatusCode = 400,
                    Body = response,
                    Message = "Bad Email or password"
                });
            Response.Headers.Add("Vf-Code", response);
            return Ok(new ResponseModel<string>
            {
                StatusCode = 200,
                Body = response,
                Message = "Signed Up"
            });

            #region Confirm With Email
            //var url = Url.Action(nameof(ConfirmEmail), "Authentication");
            //var res = await _userServices.SignUp(model, url);

            //return res ? Ok("All is good") : BadRequest("Something went Wrong"); 
            #endregion
        }

        [HttpPost("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var res = await _userServices.ConfirmEmail(email, token);
            return res ? Ok("Ur Email Has Been Verify") : BadRequest("Something went Wrong");
        }

        [HttpPost("ConfirmPhoneNumber")]
        public async Task<ActionResult> ConfirmPhoneNumber([FromBody] ConfirmPhoneNumberDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _userServices.ConfirmPhoneNumber(model.PhoneNumber, model.RealCode, model.VerifactionCode);
            return res ? Ok("Ur PhoneNumber Has Been Verify") : BadRequest("Something went Wrong");
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult<ResponseModel<TokenModel>>> SignIn([FromBody] LogInDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _userServices.SignIn(model);

            if (response.StatusCode == 200)
                return Ok(new ResponseModel<TokenModel>
                {
                    StatusCode = response.StatusCode,
                    Message = response.Message,
                    Body = response.TokenModel
                });

            if (response.StatusCode == 400)
                return BadRequest(new ResponseModel<TokenModel>
                {
                    StatusCode = response.StatusCode,
                    Message = response.Message,
                    Body = response.TokenModel
                });

            if (response.StatusCode == 404)
                return NotFound(new ResponseModel<TokenModel>
                {
                    StatusCode = response.StatusCode,
                    Message = response.Message,
                    Body = response.TokenModel
                });

            return Unauthorized(new ResponseModel<TokenModel>
            {
                StatusCode = 401,
                Message = response.Message,
                Body = response.TokenModel
            });
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet("AllByUser")]
        public async Task<ActionResult<ResponseModel<List<ReturnedTicketDto>>>> GetAllTicketsByUserId()
        {
            var tikets = await _ticketServices.GetAllTicketsByUserId(Guid.Parse(GetUserIdFromClaims()));
            return Ok(new ResponseModel<List<ReturnedTicketDto>>
            {
                StatusCode = 200,
                Body = tikets.Convert(),
                Message = "Done"
            });
        }

        [Authorize(Roles = Roles.User)]
        [HttpPost]
        public async Task<ActionResult<ResponseModel<List<ReturnedTicketDto>>>> BookTicket(TicketDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<List<ReturnedTicketDto>>
                {
                    StatusCode = 400,
                    Message = "Input is invalid"
                });
            var ticket = await _ticketServices.BookTicket(model, GetUserIdFromClaims());
            return Ok(new ResponseModel<ReturnedTicketDto>
            {
                Message = "Done Booking",
                StatusCode = 200,
                Body = ticket.ConvertToDto()
            });
        }

        private string GetUserIdFromClaims()
        {
            var id = User.Claims.FirstOrDefault(c => c.ValueType.Equals("Id"));
            return id.Value;
        }
    }
}
