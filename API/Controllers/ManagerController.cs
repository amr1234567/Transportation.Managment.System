using Core.Constants;
using Core.Dto.Identity;
using Core.Dto;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IdentityServices;
using Interfaces.IApplicationServices;
using Core.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.BusStopManager)]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IBusServices _busServices;
        private readonly IManagerServices _managerServices;

        public ManagerController(IBusServices busServices, IManagerServices managerServices)
        {
            _busServices = busServices;
            _managerServices = managerServices;
        }
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
        [HttpPost]
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
    }
}
