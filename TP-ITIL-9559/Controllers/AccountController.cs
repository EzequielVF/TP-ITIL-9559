using TP_ITIL_9559.Data;
using TP_ITIL_9559.Data.Domain;
using TP_ITIL_9559.Model;
using Microsoft.AspNetCore.Mvc;

namespace TP_ITIL_9559.Controllers
{ 
    public class AccountController : Controller
    {
            public ITILDbContext DbContext {get;set;}
            public AccountController(ITILDbContext dbContext)
            {
                DbContext = dbContext;
            }
            
        [HttpPost("/account/login")]
        public async Task<IActionResult> Login([FromBody] UserDto user)
        {
            var email = user.email?.ToLower();

            var currentUser = DbContext.Users.SingleOrDefault(p => p.Email == email && p.Password == user.password);

            if (currentUser == null) 
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            Response.Headers.Add("user-id", currentUser.Id.ToString());

            return Ok(new
            {
                userId= currentUser.Id
            });
        }
    }
}