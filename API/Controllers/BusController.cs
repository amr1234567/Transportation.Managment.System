﻿using Core.Dto;
using Core.Models;
using Interfaces.IApplicationServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusController : ControllerBase
    {
        private readonly IBusServices _busService;

        public BusController(IBusServices busService)
        {
            _busService = busService;
        }


        // GET: api/<BusController>
        [HttpGet]
        public async Task<ActionResult<List<Bus>>> Get()
        {
            return await _busService.GetAllBuses();
        }

        // GET api/<BusController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BusController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BusDto model)
        {
            await _busService.AddBus(model);
            return Ok();
        }

        // PUT api/<BusController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BusController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
