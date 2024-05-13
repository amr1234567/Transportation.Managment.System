using Core.Constants;
using Core.Dto.UserOutput;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

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

        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedBusStopDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedBusStopDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet("get-all-related-by-start/{StartBusStop}")]
        public async Task<ActionResult<ResponseModel<IEnumerable<ReturnedBusStopDto>>>> GetAllRelatedBusStops([Required] string StartBusStop)
        {
            try
            {
                if (Guid.TryParse(StartBusStop, out _))
                    ModelState.AddModelError("StartBusStop", "Id is invalid");
                var records = await _managerServices.GetAllDestinationBusStops(StartBusStop);
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

        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedBusStopDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ReturnedBusStopDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet("get-all")]
        public async Task<ActionResult<ResponseModel<IEnumerable<ReturnedBusStopDto>>>> GetAllStartBusStops()
        {
            try
            {
                var records = await _managerServices.GetAllStartBusStops();
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

        [ProducesResponseType(typeof(ResponseModel<ReturnedBusStopDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpGet("get-Bus-stop/{BusStopId}")]
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
