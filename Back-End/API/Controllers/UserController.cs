using Core.Constants;
using Core.Dto.Identity;
using Core.Dto.UserInput;
using Core.Dto.UserOutput;
using Interfaces.IApplicationServices;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using API.Filters;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    //[ValidationFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserServices userServices, ITicketServices ticketServices) : ControllerBase
    {
        private readonly IUserServices _userServices = userServices;
        private readonly ITicketServices _ticketServices = ticketServices;

        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpPost("sign-up")]
        public async Task<ActionResult<ResponseModel<IEnumerable<ErrorModelState>>>> SignUp([FromBody] SignUpDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = await _userServices.SignUp(model);
                if (response.StatusCode != 200)
                    return BadRequest(response);
                Log.Information($"Sign Up Done Successfully");
                return Ok(response);

                #region Confirm With Email
                //var url = Url.Action(nameof(ConfirmEmail), "Authentication");
                //var res = await _userServices.SignUp(model, url);

                //return res ? Ok("All is good") : BadRequest("Something went Wrong"); 
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error($"Sign Up Failed ({ex.Message})");
                return BadRequest(new ResponseModel<string>
                {
                    StatusCode = 400,
                    Message = "Failed " + ex.Message
                });
            }
        }

        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [NonAction]
        [HttpPost("confirm-email")]
        public async Task<ActionResult<string>> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            try
            {
                var res = await _userServices.ConfirmEmail(email, token);

                Log.Information($"Confirming email succeeded");

                return res ? Ok("Ur Email Has Been Verify") : BadRequest("Something went Wrong");
            }
            catch (Exception ex)
            {
                Log.Error($"Confirming email has failed ({ex.Message})");
                return BadRequest(ex.Message);
            }
        }


        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        [HttpPost("confirm-phone-number")]
        public async Task<ActionResult> ConfirmPhoneNumber([FromBody] ConfirmPhoneNumberDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var res = await _userServices.ConfirmPhoneNumber(model.Email,model.PhoneNumber, model.VerifactionCode);
                Log.Information($"Confirming phone number succeeded");

                return res ? Ok("Ur PhoneNumber Has Been Verify") : BadRequest("Something went Wrong");
            }
            catch (Exception ex)
            {
                Log.Error($"Confirming phone number has failed ({ex.Message})");
                return BadRequest($"Something went Wrong ({ex.Message})");
            }
        }

        [ProducesResponseType(typeof(ResponseModel<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpPost("sign-in")]
        public async Task<ActionResult<ResponseModel<TokenModel>>> SignIn([FromBody] LogInDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var response = await _userServices.SignIn(model);

                if (response.StatusCode == 200)
                {
                    Log.Information($"Sign In Done successfully");
                    return Ok(new ResponseModel<TokenModel>
                    {
                        StatusCode = response.StatusCode,
                        Message = response.Message,
                        Body = response.TokenModel
                    });
                }


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
            catch (Exception ex)
            {
                Log.Error($"Sign In Failed ({ex.Message})");

                return BadRequest(new ResponseModel<TokenModel>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedTicketDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedTicketDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedTicketDto>>), StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = Roles.User)]
        [HttpGet("get-all-tickets-by-user")]
        public async Task<ActionResult<ResponseModel<IEnumerable<ReturnedTicketDto>>>> GetAllTicketsByUserId()
        {
            try
            {
                var tikets = await _ticketServices.GetAllTicketsByUserId(GetUserIdFromClaims());
                Log.Information($"Get All Tickets By User Id Done successfully");

                return Ok(new ResponseModel<IEnumerable<ReturnedTicketDto>>
                {
                    StatusCode = 200,
                    Body = tikets,
                    Message = "Done"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Tickets By User Id Failed  ({ex.Message})");

                return BadRequest(new ResponseModel<IEnumerable<ReturnedTicketDto>>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = Roles.User)]
        [HttpPost("book-ticket")]
        public async Task<ActionResult<ResponseModel<bool>>> BookTicket(TicketDto model)
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
                var ticket = await _ticketServices.BookTicket(model, GetUserIdFromClaims());
                Log.Information($"Book Ticket Done successfully");

                return Ok(new ResponseModel<bool>
                {
                    Message = "Done Booking",
                    StatusCode = 200,
                    Body = true
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Book Ticket Failed  ({ex.Message})");

                return BadRequest(new ResponseModel<bool>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpPost("forget-password-verify")]
        public async Task<ActionResult<ResponseModel<bool>>> ForgetPasswordVerify([DataType(DataType.PhoneNumber), Required] string PhoneNumber)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Bad model");
                var response = await _userServices.ResetPassword(PhoneNumber);
                if (response.StatusCode == 200)
                {
                    Log.Information($"Forget Password Verification Done successfully");

                    return Ok(new ResponseModel<bool>
                    {
                        Message = "Verification Code has been sent",
                        StatusCode = 200,
                    });
                }
                Log.Information($"Forget Password Verification Failed, Please Try Again");

                return BadRequest(new ResponseModel<bool>
                {
                    Message = response.Message,
                    StatusCode = response.StatusCode,
                    Body = response.Body
                });
            }
            catch (Exception ex)
            {
                Log.Error($"ForgetPasswordVerify Failed  ({ex.Message})");

                return BadRequest(new ResponseModel<bool>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Body = true
                });
            }
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpPost("reset-Password")]
        public async Task<ActionResult<ResponseModel<bool>>> ResetPassword(ResetPasswordDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Bad model");
                var token = await _userServices.ResetPasswordConfirmation(model);

                Log.Information($"reset Password  Done successfully");

                return Ok(new ResponseModel<bool>
                {
                    Message = "Password has been reset",
                    StatusCode = 200,
                });
            }
            catch (Exception ex)
            {
                Log.Error($"ForgetPasswordVerify Failed  ({ex.Message})");

                return BadRequest(new ResponseModel<bool>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = Roles.User)]
        [HttpPost("edit-personal-data")]
        public async Task<ActionResult<ResponseModel<bool>>> EditPersonalData(EditPersonalDataDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userServices.EditPersonalData(model, GetUserIdFromClaims());
                if (response.StatusCode == 200)
                    return Ok(response);

                else if (response.StatusCode == 400)
                    return BadRequest(response);
                else if (response.StatusCode == 500)
                    return StatusCode(response.StatusCode);
            }
            return BadRequest(new ResponseModel<bool>
            {
                Message = "Input is invalid",
                StatusCode = 400,
                Body = false
            });
        }

        private string GetUserIdFromClaims()
        {
            return User.FindFirstValue("Id");
        }
    }
}
