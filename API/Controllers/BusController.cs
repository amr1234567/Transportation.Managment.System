using Core.Constants;
using Core.Dto;
using Core.Dto.UserOutput;
using Core.Models;
using Interfaces.IApplicationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Roles.BusStopManager},{Roles.Admin}")]
    public class BusController : ControllerBase
    {
        private readonly IBusServices _busService;

        public BusController(IBusServices busService)
        {
            _busService = busService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<ResponseModel<IEnumerable<Bus>>>> GetAllBuses()
        {
            try
            {
                var model = new ResponseModel<IEnumerable<Bus>>
                {

                    StatusCode = 200,
                    Message = "Done",
                    Body = await _busService.GetAllBuses(),

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

        [HttpGet("get-bus/{BusId}")]
        public async Task<ActionResult<ResponseModel<Bus>>> GetBus([FromRoute] Guid BusId)
        {
            try
            {
                var bus = await _busService.GetBusById(BusId);
                var model = new ResponseModel<Bus>
                {
                    StatusCode = 200,
                    Message = "Done",
                    Body = bus
                };
                Log.Information($"get Bus By Id Success: {bus}");
                return model;
            }
            catch (Exception ex)
            {
                var model = new ResponseModel<Bus>
                {
                    StatusCode = 400,
                    Message = $"Get Bus By Id Error occurred :{ex.Message}"
                };
                Log.Error($"get Bus By Id failed: {ex.Message}");
                return model;
            }




        }

        [HttpPut("edit-bus/{BusId}")]
        public async Task<ActionResult<ResponseModel<Bus>>> EditBus([FromRoute] Guid BusId, [FromBody] BusDto model)
        {
            try
            {
                var res = await _busService.EditBus(BusId, model);
                Log.Information($"edit Bus Success: {res}");
                if (res.StatusCode == 200)
                    return Ok(res);
                Log.Error($"edit Bus failed");
                return BadRequest();
            }
            catch (Exception ex)
            {
                Log.Error($"edit Bus failed: {ex.Message}");
                return BadRequest($"edit Bus failed: {ex.Message}");
            }
        }
    }
}
