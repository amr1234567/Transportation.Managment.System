using Core.Dto.Identity;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        // POST api/<UserController>
        [HttpPost("SignUp")]
        public async Task<ActionResult> SignUp([FromBody] SignUpDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _userServices.SignUp(model);
            if (string.IsNullOrEmpty(response))
                return BadRequest();
            Response.Headers.Add("Vf-Code", response);
            return Ok();
            //var url = Url.Action(nameof(ConfirmEmail), "Authentication");
            //var res = await _userServices.SignUp(model, url);

            //return res ? Ok("All is good") : BadRequest("Something went Wrong");
        }

        // PUT api/<UserController>/5
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
        public async Task<ActionResult> SignIn([FromBody] LogInDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _userServices.SignIn(model);
            if (response.StatusCode == 200)
                return Ok(response);
            if (response.StatusCode == 400)
                return BadRequest(response);
            if (response.StatusCode == 404)
                return NotFound(response);
            return Unauthorized(response.Message);
        }
    }
}
