using Core.Constants;
using Core.Dto.UserOutput;
using Core.Models;
using Interfaces.IApplicationServices;
using Interfaces.IIdentityServices;
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
        private readonly IManagerServices _managerServices;

        public BusStopsController(IManagerServices managerServices)
        {
            _managerServices = managerServices;
        }
        [HttpGet]

        public async Task<ActionResult<ResponseModel<List<BusStopDto>>>> GetAllBusStops()
        {
           var records= await _managerServices.GetAllBusStops();
            return Ok(new ResponseModel<List<BusStopDto>>()
            {
                Body = records,
                Message = "Done",
                StatusCode = 200
            });
        }


        [HttpGet("AllBuses/{id}")]
        public async Task<ActionResult<List<Bus>>> GetBusStopById(string id)
        {
            var BusStop = await _managerServices.GetBusStop(id);
            if (BusStop == null)
                return BadRequest("Wrong Id");
            return Ok(BusStop);
        }
    }
}
