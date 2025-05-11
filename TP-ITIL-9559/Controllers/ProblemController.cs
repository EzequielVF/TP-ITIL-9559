using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP_ITIL_9559.Data;
using TP_ITIL_9559.Data.Domain;
using TP_ITIL_9559.Model;

namespace TP_ITIL_9559.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProblemController : ControllerBase
    {
        public ITILDbContext DbContext { get; set; }
        public ProblemController(ITILDbContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpPost("create")]
        public IActionResult SaveProblem([FromBody] ProblemDto problemDto)
        {
            if (ModelState.IsValid)
            {
                var user = DbContext.Users.SingleOrDefault(u => u.Id == problemDto.userId);
                var configurationItem = DbContext.Configuration.SingleOrDefault(c => c.Id == problemDto.configurationItemId);
                var assignedUser = DbContext.Users.SingleOrDefault(u => u.Id == problemDto.assignedUserId);
                var incidents = DbContext.Incidents.Include(i => i.Problems).Where(i => problemDto.incidentIds.Contains(i.Id)).ToList();
                var problem = new Problem()
                {
                    Title = problemDto.title,
                    Description = problemDto.description,
                    CreatedDate = DateTime.UtcNow,
                    UserId = problemDto.userId,
                    User = user,
                    ConfigurationItemId = problemDto.configurationItemId,
                    ConfigurationItem = configurationItem,
                    AssignedUserId = problemDto.assignedUserId,
                    AssignedUser = assignedUser,
                    Impact = problemDto.impact,
                    Priority = problemDto.priority,
                    Incidents = incidents
                };

                foreach (var incident in incidents)
                {
                    incident.Problems.Add(problem);
                }

                DbContext.Problems.Add(problem);
                DbContext.SaveChanges();

                var response = new
                {
                    problem.Id,
                    problem.Title,
                    problem.Description,
                    problem.CreatedDate,
                    User = new { user.Id, user.Email },
                    ConfigurationItem = new { configurationItem.Id, configurationItem.Title },
                    AssignedUser = new { assignedUser.Id, assignedUser.Email },
                    Incidents = incidents.Select(i => new { i.Id, i.Title })
                };

                return Ok(response);
            }

            return BadRequest();
        }
    }
}
