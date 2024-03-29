using IServices.IServices;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Transportation_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneyController : ControllerBase
    {
        private readonly IJourneyServices _journeyService;

        public JourneyController(IJourneyServices journeyService)
        {
            _journeyService = journeyService;
        }
        // GET: api/<JournyController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<JournyController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<JournyController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<JournyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<JournyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
