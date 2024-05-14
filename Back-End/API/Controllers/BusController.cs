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
    public class BusController(IBusServices busService) : ControllerBase
    {
        private readonly IBusServices _busService = busService;

        [ProducesResponseType(typeof(ResponseModel<Bus>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpGet("get-bus/{BusId}")]
        public async Task<ActionResult> GetBus([FromRoute] Guid BusId)
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
                return Ok(model);
            }
            catch (Exception ex)
            {
                var model = new ResponseModel<Bus>
                {
                    StatusCode = 400,
                    Message = $"Get Bus By Id Error occurred :{ex.Message}"
                };
                Log.Error($"get Bus By Id failed: {ex.Message}");
                return BadRequest(model);
            }
        }

        [ProducesResponseType(typeof(ResponseModel<Bus>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<ErrorModelState>>), StatusCodes.Status400BadRequest)]
        [HttpPut("edit-bus/{BusId}")]
        public async Task<ActionResult> EditBus([FromRoute] Guid BusId, [FromBody] BusDto model)
        {
            try
            {
                var res = await _busService.EditBus(BusId, model);
                Log.Information($"edit Bus Success");
                if (res.StatusCode == 200)
                    return Ok(res);
                Log.Error($"edit Bus failed");
                return BadRequest(res);
            }
            catch (Exception ex)
            {
                Log.Error($"edit Bus failed: {ex.Message}");
                return BadRequest($"edit Bus failed: {ex.Message}");
            }
        }
    }
}
