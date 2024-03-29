﻿using IServices.IServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Transportation_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusStopController : ControllerBase
    {
        private readonly IBusStopServices _busStopService;

        public BusStopController(IBusStopServices busStopService)
        {
            _busStopService = busStopService;
        }
        // GET: api/<BusStopController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BusStopController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BusStopController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BusStopController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BusStopController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
