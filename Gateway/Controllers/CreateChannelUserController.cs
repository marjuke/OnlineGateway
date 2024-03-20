using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.ServiceModel.Security;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateChannelUserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        public CreateChannelUserController(DataContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: api/<CreateChannelUserController>
        [HttpGet]
        public ActionResult Get()
        {
            var d = _userManager.Users.Include(c => c.ChannelList).FirstOrDefault();
            return Ok(_userManager.Users.Include(c => c.ChannelList).FirstOrDefault());
        }

        // GET api/<CreateChannelUserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CreateChannelUserController>
        [HttpPost]
        public async Task<ActionResult<AppUser>> Post(string userName, string userID,string email,string password)
        {
            //var bKash = _context.ChannelList.FirstOrDefault();
            
            var user = new AppUser
            {
                
                ChannelListID = 1,UserName = userName,
                UserID=userID,
                Email=email,
                EmailConfirmed=false,
                PhoneNumberConfirmed=false,
                TwoFactorEnabled=false,
                LockoutEnabled=false,
                AccessFailedCount=0,
                
                
            };
            var result =await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return Ok(_userManager.Users.Include(c=>c.ChannelList).Where(s=>s.UserID == userID).FirstOrDefault());
            }else { return BadRequest(); }

        }

        // PUT api/<CreateChannelUserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CreateChannelUserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
