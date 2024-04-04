using Core.Constants;
using Core.Models;
using Interfaces.IApplicationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Roles.BusStopManager},{Roles.User},{Roles.Admin}")]
    public class BusStopsController : ControllerBase
    {
        private readonly IBusStopServices _busStopServices;

        public BusStopsController(IBusStopServices busStopServices)
        {
            _busStopServices = busStopServices;
        }
        [HttpGet]
        public async Task<ActionResult<List<Bus>>> GetAllBusStops()
        {
            return Ok(await _busStopServices.GetAllBusStops());
        }


        [HttpGet("AllBuses/{id}")]
        public async Task<ActionResult<List<Bus>>> GetAllBusesInBusStop(Guid id)
        {
            var buses = await _busStopServices.GetBusesByBusStopId(id);
            if (buses == null)
                return BadRequest("Wrong Id");
            return Ok(buses);
        }
    }
}
