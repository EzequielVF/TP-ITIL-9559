
using TP_ITIL_9559.Data;
using TP_ITIL_9559.Data.Domain;
using TP_ITIL_9559.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TP_ITIL_9559.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentController : ControllerBase
    {
        public ITILDbContext DbContext { get; set; }
        public IncidentController(ITILDbContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpPost("create")]
        public IActionResult SaveIncident([FromBody] IncidentDto incident)
        {
            if (ModelState.IsValid)
            {
                var user = DbContext.Users.SingleOrDefault(u => u.Id == incident.UserId);
                var configurationItem = DbContext.Configuration.SingleOrDefault(c => c.Id == incident.ConfigurationItemId);
                var assignedUser = DbContext.Users.SingleOrDefault(u => u.Id == incident.AssignedUserId);
                DbContext.Incidents.Add(new Incident()
                {
                    Title = incident.Title,
                    Description = incident.Description,
                    CreatedDate = DateTime.UtcNow,
                    UserId = incident.UserId,
                    User = user,
                    ConfigurationItemId = incident.ConfigurationItemId,
                    ConfigurationItem = configurationItem,
                    RootCause = incident.RootCause,
                    LastUpdated = DateTime.UtcNow,
                    ClientName = incident.ClientName,
                    ClientEmail = incident.ClientEmail,
                    State = State.ABIERTO,
                    AssignedUserId = incident.AssignedUserId,
                    AssignedUser = assignedUser,
                    Impact = incident.Impact,
                    Priority = incident.Priority
                });

                DbContext.SaveChanges();
                return Ok(incident);
            }

            return BadRequest();
        }

        [HttpPatch("{incidentId}")]
        public IActionResult UpdateIncident([FromBody] IncidentDto modifiedIncident, long incidentId)
        {
            if (ModelState.IsValid)
            {
                var incident = DbContext.Incidents.Include(i => i.AssignedUser).SingleOrDefault(i => i.Id == incidentId);
                var assignedUser = DbContext.Users.SingleOrDefault(u => u.Id == modifiedIncident.AssignedUserId);
                if (incident != null)
                {
                    incident.Title = modifiedIncident.Title;
                    incident.Description = modifiedIncident.Description;
                    incident.State = modifiedIncident.State;
                    incident.AssignedUserId = modifiedIncident.AssignedUserId;
                    incident.AssignedUser = assignedUser;
                    incident.Impact = modifiedIncident.Impact;
                    incident.Priority = modifiedIncident.Priority;
                    incident.LastUpdated = DateTime.UtcNow;
                    incident.RootCause = modifiedIncident.RootCause;
                    if (incident.State == State.IMPLEMENTADO) { incident.ClosedDate = DateTime.UtcNow; }
                    DbContext.Incidents.Update(incident);
                    DbContext.SaveChanges();
                    return Ok(incident);
                }
            }
            return BadRequest();
        }

        [HttpGet("")]
        public IActionResult Incidents()
        {
            return Ok(DbContext.Incidents
            .Include(i => i.ConfigurationItem)
            .Include(i => i.AssignedUser)
            .OrderByDescending(i => i.CreatedDate));
        }

        [HttpGet("{incidentId}")]
        public IActionResult IncidentInfo(long incidentId)
        {
            var incident = DbContext.Incidents
            .SingleOrDefault(i => i.Id == incidentId);
            if (incident != null)
            {
                var configurationItem = DbContext.Configuration.SingleOrDefault(c => c.Id == incident.ConfigurationItemId);
                if (configurationItem != null)
                {
                    incident.ConfigurationItem = configurationItem;
                }

                var userInfo = DbContext.Users.SingleOrDefault(u => u.Id == incident.UserId);
                if (userInfo != null)
                {
                    incident.User = userInfo;
                }

                return Ok(incident);
            }
            return NotFound($"{incidentId} not found");
        }

        [HttpDelete("{incidentId}")]
        public IActionResult DeleteIncident(long incidentId)
        {
            var incident = DbContext.Incidents.SingleOrDefault(i => i.Id == incidentId);
            if (incident != null)
            {
                DbContext.Incidents.Remove(incident);
                DbContext.SaveChanges();
                return Ok($"Incident {incidentId} deleted succesfuly");
            }

            return NotFound($"{incidentId} not found");
        }

        [HttpGet("{incidentId}/relateditems")]
        public IActionResult GetRelatedItems(long incidentId)
        {
            var items = DbContext.Incidents.Include(i => i.ConfigurationItem).SingleOrDefault(i => i.Id == incidentId).ConfigurationItem;
            if (items == null)
            {
                return NotFound($"item not found for incident {incidentId}");
            }
            return Ok(items);
        }

        [HttpPost("{incidentId}/comments")]
        public IActionResult SaveComment(long incidentId, [FromBody] string comment)
        {
            var incident = DbContext.Incidents.SingleOrDefault(i => i.Id == incidentId);
            if (incident == null)
            {
                return NotFound();
            }
            incident.Comments.Add(comment);

            DbContext.SaveChanges();

            return Ok(comment);
        }

        [HttpGet("{incidentId}/comments")]
        public IActionResult Comments(long incidentId)
        {
            var incident = DbContext.Incidents.SingleOrDefault(i => i.Id == incidentId);
            if (incident == null)
            {
                return NotFound();
            }
            return Ok(new { Comments = incident.Comments });
        }

        [HttpDelete("{incidentId}/comments/{commentIndex}")]
        public IActionResult DeleteComment(long incidentId, int commentIndex)
        {
            var incident = DbContext.Incidents.SingleOrDefault(i => i.Id == incidentId);
            if (incident == null)
            {
                return NotFound();
            }

            if (commentIndex < 0 || commentIndex >= incident.Comments.Count)
            {
                return NotFound();
            }

            incident.Comments.RemoveAt(commentIndex);

            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpGet("getincidents")]
        public List<Incident> GetIncidents()
        {
            var incidents = DbContext.Incidents
            .OrderByDescending(i => i.CreatedDate);
            return incidents.ToList();
        }
    }
}
