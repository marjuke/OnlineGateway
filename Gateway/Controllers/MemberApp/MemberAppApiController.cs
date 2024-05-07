using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gateway.Controllers.MemberApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberAppApiController : ControllerBase
    {
        // GET: api/<MemberAppApiController>
        [Authorize]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MemberAppApiController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MemberAppApiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MemberAppApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MemberAppApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
