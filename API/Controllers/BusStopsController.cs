using Core.Constants;
using Core.Dto.UserOutput;
using Core.Models;
using Interfaces.IApplicationServices;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Twilio.TwiML.Voice;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Roles.BusStopManager},{Roles.User},{Roles.Admin}")]
    public class BusStopsController : ControllerBase
    {
        private readonly IManagerServices _managerServices;

        public BusStopsController(IManagerServices managerServices)
        {
            _managerServices = managerServices;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<ResponseModel<IEnumerable<ReturnedBusStopDto>>>> GetAllBusStops()
        {
            try
            {
                var records = await _managerServices.GetAllBusStops();
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
                Log.Error($"Get All BusStops Failed :{ex.Message}");
                return Ok(new ResponseModel<IEnumerable<BusStopDto>>()
                {
                    Body = new List<BusStopDto>(),
                    Message = $"Get All BusStops Failed :{ex.Message}",
                    StatusCode = 400
                });
            }

        }

        [HttpGet("get-Bus-stop/{id}")]
        public async Task<ActionResult<ReturnedBusStopDto>> GetBusStopById(string BusStopId)
        {
            try
            {
                var BusStop = await _managerServices.GetBusStop(BusStopId);

                Log.Information("Get  BusStop By Id succeeded");
                if (BusStop == null)
                    return BadRequest("Wrong Id");
                Log.Error($"Get  BusStop By Id Failed");
                return Ok(BusStop);
            }
            catch (Exception ex)
            {
                Log.Error($"Get  BusStop By Id Failed :{ex.Message}");
                return BadRequest($"Get  BusStop By Id Failed :{ex.Message}");
            }
        }
    }
}
