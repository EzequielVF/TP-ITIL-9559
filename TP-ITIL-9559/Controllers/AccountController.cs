using TP_ITIL_9559.Data;
using TP_ITIL_9559.Data.Domain;
using TP_ITIL_9559.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using TP_ITIL_9559.Sys;

namespace TP_ITIL_9559.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        public ITILDbContext DbContext { get; set; }
        public AccountController(ITILDbContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto user)
        {
            var email = user.email?.ToLower();

            var currentUser = DbContext.Users.SingleOrDefault(p => p.Email == email && p.Password == user.password);

            if (currentUser == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
            };
            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userIdentity));

            Response.Headers.Add("user-id", currentUser.Id.ToString());

            return Ok(new
            {
                userId = currentUser.Id,
                group = currentUser.Group
            });
        }

        [HttpGet("users")]
        public IActionResult Users()
        {
            return Ok(DbContext.Users.ToList().Select(x => x.Email));
        }

        [HttpGet("clients")]
        public IActionResult Clients()
        {
            return Ok(DbContext.Clients.ToList());
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto user)
        {
            var email = user.email?.ToLower();

            var currenUser = DbContext.Users.SingleOrDefault(p => p.Email == email);

            if (currenUser != null)
            {
                throw new BusinessException($"El email {email} ya está registrado.");
            }

            DbContext.Users.Add(new User
            {
                Email = email,
                Password = user.password
            });

            DbContext.SaveChanges();

            return Accepted();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var email = User.Identity.Name;

            var user = DbContext.Users.SingleOrDefault(u => u.Email == email);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                group = user.Group
            });
        }

    }
}