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

        [HttpGet("")]
        public IActionResult Problems()
        {
            return Ok(DbContext.Problems
                .Include(p => p.AssignedUser)
                .Include(p => p.Incidents)
                .OrderByDescending(p => p.CreatedDate)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Description,
                    p.CreatedDate,
                    p.ConfigurationItem,
                    AssignedUser = p.AssignedUser != null ? new { p.AssignedUser.Id, p.AssignedUser.Email } : null,
                    Incidents = p.Incidents.Select(i => new { i.Id, i.Title })
                }));
        }

        [HttpDelete("{problemId}")]
        public IActionResult DeleteProblem(long problemId)
        {
            var problem = DbContext.Problems.SingleOrDefault(i => i.Id == problemId);
            if (problem != null)
            {
                DbContext.Problems.Remove(problem);
                DbContext.SaveChanges();
                return Ok($"Incident {problemId} deleted succesfuly");
            }

            return NotFound($"{problemId} not found");
        }

        [HttpGet("{problemId}")]
        public IActionResult ProblemInfo(long problemId)
        {
            var problem = DbContext.Problems.Include(i => i.AssignedUser).Include(i => i.ConfigurationItem).SingleOrDefault(i => i.Id == problemId);
            if (problem != null)
            {
                var configurationItem = DbContext.Configuration.SingleOrDefault(c => c.Id == problem.ConfigurationItemId);
                if (configurationItem != null)
                {
                    problem.ConfigurationItem = configurationItem;
                }

                var userInfo = DbContext.Users.SingleOrDefault(u => u.Id == problem.UserId);
                if (userInfo != null)
                {
                    problem.User = userInfo;
                }

                return Ok(problem);
            }
            return NotFound($"{problemId} not found");

        }

        [HttpPatch("{problemId}")]
        public IActionResult UpdateProblem([FromBody] ProblemDto modifiedProblem, long problemId)
        {
            if (ModelState.IsValid)
            {
                var user = DbContext.Users.SingleOrDefault(u => u.Id == modifiedProblem.userId);
                var configurationItem = DbContext.Configuration.SingleOrDefault(c => c.Id == modifiedProblem.configurationItemId);
                var problem = DbContext.Problems.Include(i => i.AssignedUser).SingleOrDefault(i => i.Id == problemId);
                var assignedUser = DbContext.Users.SingleOrDefault(u => u.Id == modifiedProblem.assignedUserId);
                var incidents = DbContext.Incidents.Include(i => i.Problems).Where(i => modifiedProblem.incidentIds.Contains(i.Id)).ToList();
                if (problem != null)
                {
                    problem.Title = modifiedProblem.title;
                    problem.Description = modifiedProblem.description;
                    problem.AssignedUserId = modifiedProblem.assignedUserId;
                    problem.AssignedUser = assignedUser;
                    problem.Impact = modifiedProblem.impact;
                    problem.Priority = modifiedProblem.priority;
                    DbContext.Problems.Update(problem);
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
            }
            return BadRequest();
        }

        [HttpPost("{problemId}/comments")]
        public IActionResult SaveComment(long problemId, [FromBody] string comment)
        {
            var problem = DbContext.Problems.SingleOrDefault(i => i.Id == problemId);
            if (problem == null)
            {
                return NotFound();
            }
            problem.Comments.Add(comment);

            DbContext.SaveChanges();

            return Ok(comment);
        }

        [HttpGet("{problemId}/comments")]
        public IActionResult Comments(long problemId)
        {
            var problem = DbContext.Problems.SingleOrDefault(i => i.Id == problemId);
            if (problem == null)
            {
                return NotFound();
            }
            return Ok(new { Comments = problem.Comments });
        }

        [HttpDelete("{problemId}/comments/{commentIndex}")]
        public IActionResult DeleteComment(long problemId, int commentIndex)
        {
            var problem = DbContext.Problems.SingleOrDefault(i => i.Id == problemId);
            if (problem == null)
            {
                return NotFound();
            }

            if (commentIndex < 0 || commentIndex >= problem.Comments.Count)
            {
                return NotFound();
            }

            problem.Comments.RemoveAt(commentIndex);

            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpGet("{problemId}/incidents")]
        public IActionResult ProblemIncidents(long problemId)
        {
            var problem = DbContext.Problems.Include(i => i.Incidents).SingleOrDefault(i => i.Id == problemId);
            if (problem != null)
            {
                return Ok(problem.Incidents);
            }
            return NotFound();
        }

        [HttpPatch("{problemId}/incidents/{incidentId}")]
        public IActionResult AddIncident(long problemId, long incidentId)
        {
            var problem = DbContext.Problems.Include(i => i.Incidents).SingleOrDefault(i => i.Id == problemId);
            var incident = DbContext.Incidents.Include(i => i.Problems).SingleOrDefault(i => i.Id == incidentId);
            if (problem != null && incident != null)
            {
                problem.Incidents.Add(incident);
                incident.Problems.Add(problem);
                DbContext.SaveChanges();
                return Ok(incident);
            }
            return NotFound();
        }

        [HttpDelete("{problemId}/incidents/{incidentId}")]
        public IActionResult RemoveIncident(long problemId, long incidentId)
        {
            var problem = DbContext.Problems.Include(i => i.Incidents).SingleOrDefault(i => i.Id == problemId);
            var incident = DbContext.Incidents.Include(i => i.Problems).SingleOrDefault(i => i.Id == incidentId);
            if (problem != null && incident != null)
            {
                problem.Incidents.Remove(incident);
                incident.Problems.Remove(problem);
                DbContext.SaveChanges();
                return Ok(incident);
            }
            return NotFound();
        }

        [HttpGet("getproblems")]
        public List<Problem> GetProblems()
        {
            var problems = DbContext.Problems
            .OrderByDescending(i => i.CreatedDate);
            return problems.ToList();
        }
    }
}
