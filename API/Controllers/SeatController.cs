using Interfaces.IApplicationServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatServices _seatService;

        public SeatController(ISeatServices seatService)
        {
            _seatService = seatService;
        }
        // GET: api/<SeatController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SeatController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SeatController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SeatController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SeatController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
