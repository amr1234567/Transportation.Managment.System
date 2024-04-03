using Core.Constants;
using Core.Dto;
using Core.Dto.Identity;
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
    public class AdminController(IAdminServices adminServices, IManagerServices managerServices) : ControllerBase
    {
        private readonly IAdminServices _adminServices = adminServices;
        private readonly IManagerServices _managerServices = managerServices;

        //[NonAction]
        [Authorize(Roles = Roles.Admin)]
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
    }
}
