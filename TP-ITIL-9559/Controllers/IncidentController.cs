using System.Text.Json;
using System.Text.Json.Serialization;
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
        public ITILDbContext DbContext {get;set;}
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
    }
}
