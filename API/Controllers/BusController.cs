using Core.Constants;
using Core.Dto;
using Core.Models;
using Interfaces.IApplicationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<ResponseModel<List<Bus>>>> GetAllBuses()
        {
            return new ResponseModel<List<Bus>>
            {
                StatusCode = 200,
                Message = "Done",
                Body = await _busService.GetAllBuses()
            };
        }

        [HttpGet("{BusId}")]
        public async Task<ActionResult<ResponseModel<Bus>>> GetBus([FromRoute] Guid BusId)
        {
            var bus = await _busService.GetBusById(BusId);
            if (bus == null)
                return new ResponseModel<Bus>
                {
                    StatusCode = 400,
                    Message = "No bus with this id"
                };
            return new ResponseModel<Bus>
            {
                StatusCode = 200,
                Message = "Done",
                Body = bus
            };
        }

        [HttpPut("{BusId}")]
        public async Task<ActionResult<ResponseModel<Bus>>> EditBus([FromRoute] Guid BusId, [FromBody] BusDto busDto)
        {
            var res = await _busService.EditBus(BusId, busDto);
            if (res.StatusCode == 200)
                return Ok(res);
            return BadRequest(res);
        }
    }
}
