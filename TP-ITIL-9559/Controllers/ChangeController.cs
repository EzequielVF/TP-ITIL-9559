using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using TP_ITIL_9559.Data;
using TP_ITIL_9559.Data.Domain;
using TP_ITIL_9559.Model;

namespace TP_ITIL_9559.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChangeController : ControllerBase
    {
        public ITILDbContext DbContext { get; set; }
        public ChangeController(ITILDbContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpPost("create")]
        public IActionResult SaveChange([FromBody] ChangeDto changeDto)
        {
            if (ModelState.IsValid)
            {
                var user = DbContext.Users.SingleOrDefault(u => u.Id == changeDto.UserId);
                var configurationItem = DbContext.Configuration.SingleOrDefault(c => c.Id == changeDto.ConfigurationItemId);
                var assignedUser = DbContext.Users.SingleOrDefault(u => u.Id == changeDto.AssignedUserId);
                DateTime scheduled = DateTime.Parse(changeDto.ScheduledDate);
                var incidents = DbContext.Incidents.Include(i => i.Changes).Where(i => changeDto.IncidentIds.Contains(i.Id)).ToList();
                var problems = DbContext.Problems.Include(p => p.Changes).Where(p => changeDto.ProblemIds.Contains(p.Id)).ToList();

                var change = new Change()
                {
                    Title = changeDto.Title,
                    Description = changeDto.Description,
                    CreatedDate = DateTime.UtcNow,
                    UserId = changeDto.UserId,
                    User = user,
                    ConfigurationItemId = changeDto.ConfigurationItemId,
                    ConfigurationItem = configurationItem,
                    State = State.ABIERTO,
                    AssignedUserId = changeDto.AssignedUserId,
                    AssignedUser = assignedUser,
                    Impact = changeDto.Impact,
                    Priority = changeDto.Priority,
                    ScheduledDate = DateTime.SpecifyKind(scheduled, DateTimeKind.Utc),
                    Incidents = incidents,
                    Problems = problems,
                    Disabled = false
                };

                foreach (var incident in incidents)
                {
                    incident.Changes.Add(change);
                }
                foreach (var problem in problems)
                {
                    problem.Changes.Add(change);
                }

                DbContext.Changes.Add(change);
                DbContext.SaveChanges();
                return Ok(change);
            }

            return BadRequest();
        }

        [HttpPatch("{changeId}")]
        public IActionResult UpdateChange([FromBody] ChangeDto modifiedChange, long changeId)
        {
            if (ModelState.IsValid)
            {
                var change = DbContext.Changes.Include(i => i.AssignedUser).Include(i => i.ConfigurationItem).SingleOrDefault(i => i.Id == changeId);
                var assignedUser = DbContext.Users.SingleOrDefault(u => u.Id == modifiedChange.AssignedUserId);
                if (change != null)
                {
                    change.Title = modifiedChange.Title;
                    change.Description = modifiedChange.Description;
                    change.State = modifiedChange.State;
                    if (modifiedChange.State == State.IMPLEMENTADO)
                    {
                        var item = change.ConfigurationItem;
                        var history = JsonConvert.DeserializeObject<Dictionary<string, object>>(item.VersionHistory);
                        var highestVersionKey = change.ConfigurationItem.VersionId == null ? "v1.0" : change.ConfigurationItem.VersionId;
                        var newVersionKey = IncrementVersion(highestVersionKey);
                        
                        if (history == null) { history = new Dictionary<string, object>(); }
                        var newDescription = item.Description + $".\nModificado segun el cambio {change.Title}.\nActualizado de {highestVersionKey} a {newVersionKey}";
                        history[newVersionKey] = String.Format("Titulo:{0}|Descripcion:{1}", item.Title, newDescription);
                        item.VersionId = newVersionKey;
                        item.VersionHistory = JsonConvert.SerializeObject(history);
                        item.Description = newDescription;
                    }
                    change.AssignedUserId = modifiedChange.AssignedUserId;
                    change.AssignedUser = assignedUser;
                    change.Impact = modifiedChange.Impact;
                    change.Priority = modifiedChange.Priority;
                    DbContext.Changes.Update(change);
                    DbContext.SaveChanges();
                    return Ok(change);
                }
            }
            return BadRequest();
        }

        private static string IncrementVersion(string versionKey)
        {
            var key = versionKey.Length > 1 ? versionKey.Substring(1) : versionKey;
            var number = decimal.Parse(key, CultureInfo.InvariantCulture.NumberFormat);
            var newNumber = number + 0.1m;
            var newVersion = "v" + newNumber.ToString();
            return newVersion.Replace(',', '.');
        }

        [HttpGet("")]
        public IActionResult Changes()
        {
            return Ok(DbContext.Changes
            .Where(i => i.Disabled == false)
            .Include(i => i.AssignedUser)
            .Include(i => i.Incidents)
            .Include(i => i.Problems)
            .OrderByDescending(c => c.CreatedDate));
        }

        [HttpGet("{changeId}")]
        public IActionResult ChangeInfo(long changeId)
        {
            var change = DbContext.Changes
            .Include(i => i.AssignedUser)
            .Include(i => i.ConfigurationItem)
            .SingleOrDefault(i => i.Id == changeId);
            if (change != null)
            {
                var configurationItem = DbContext.Configuration.SingleOrDefault(c => c.Id == change.ConfigurationItemId);
                if (configurationItem != null)
                {
                    change.ConfigurationItem = configurationItem;
                }

                var userInfo = DbContext.Users.SingleOrDefault(u => u.Id == change.UserId);
                if (userInfo != null)
                {
                    change.User = userInfo;
                }

                return Ok(change);
            }
            return NotFound($"{changeId} not found");

        }

        [HttpDelete("")]
        public IActionResult DeleteChange(long changeId)
        {
            var change = DbContext.Changes.SingleOrDefault(i => i.Id == changeId);
            if (change != null)
            {
                change.Disabled = true;
                DbContext.Changes.Update(change);
                DbContext.SaveChanges();
                return Ok($"Incident {changeId} disabled succesfuly");
            }

            return NotFound($"{changeId} not found");
        }

        [HttpPost("{changeId}/comment")]
        public IActionResult SaveComment(long changeId, [FromBody] string comment)
        {
            var change = DbContext.Changes.SingleOrDefault(i => i.Id == changeId);
            if (change == null)
            {
                return NotFound();
            }
            change.Comments.Add(comment);

            DbContext.SaveChanges();

            return Ok(comment);
        }

        [HttpGet("{changeId}/comment")]
        public IActionResult Comments(long changeId)
        {
            var change = DbContext.Changes.SingleOrDefault(i => i.Id == changeId);
            if (change == null)
            {
                return NotFound();
            }
            return Ok(new { Comments = change.Comments });
        }

        [HttpDelete("{changeId}/comment/{commentIndex}")]
        public IActionResult DeleteComment(long changeId, int commentIndex)
        {
            var change = DbContext.Changes.SingleOrDefault(i => i.Id == changeId);
            if (change == null)
            {
                return NotFound();
            }

            if (commentIndex < 0 || commentIndex >= change.Comments.Count)
            {
                return NotFound();
            }

            change.Comments.RemoveAt(commentIndex);

            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpGet("{changeId}/incidents")]
        public IActionResult ChangeIncidents(long changeId)
        {
            var change = DbContext.Changes.Include(i => i.Incidents).SingleOrDefault(i => i.Id == changeId);
            if (change != null)
            {
                return Ok(change.Incidents);
            }
            return NotFound();
        }

        [HttpGet("{changeId}/problems")]
        public IActionResult ChangeProblems(long changeId)
        {
            var change = DbContext.Changes.Include(i => i.Problems).SingleOrDefault(i => i.Id == changeId);
            if (change != null)
            {
                return Ok(change.Problems);
            }
            return NotFound();
        }

        [HttpPatch("{changeId}/incidents/{incidentId}")]
        public IActionResult AddIncident(long changeId, long incidentId)
        {
            var change = DbContext.Changes.Include(i => i.Incidents).SingleOrDefault(i => i.Id == changeId);
            var incident = DbContext.Incidents.Include(i => i.Changes).SingleOrDefault(i => i.Id == incidentId);
            if (change != null && incident != null)
            {
                change.Incidents.Add(incident);
                incident.Changes.Add(change);
                DbContext.SaveChanges();
                return Ok(incident);
            }
            return NotFound();
        }

        [HttpDelete("{changeId}/incidents/{incidentId}")]
        public IActionResult RemoveIncident(long changeId, long incidentId)
        {
            var change = DbContext.Changes.Include(i => i.Incidents).SingleOrDefault(i => i.Id == changeId);
            var incident = DbContext.Incidents.Include(i => i.Changes).SingleOrDefault(i => i.Id == incidentId);
            if (change != null && incident != null)
            {
                change.Incidents.Remove(incident);
                incident.Changes.Remove(change);
                DbContext.SaveChanges();
                return Ok(incident);
            }
            return NotFound();
        }

        [HttpPatch("{changeId}/problems/{problemId}")]
        public IActionResult AddProblem(long changeId, long problemId)
        {
            var change = DbContext.Changes.Include(i => i.Problems).SingleOrDefault(i => i.Id == changeId);
            var problem = DbContext.Problems.Include(i => i.Changes).SingleOrDefault(i => i.Id == problemId);
            if (change != null && problem != null)
            {
                change.Problems.Add(problem);
                problem.Changes.Add(change);
                DbContext.SaveChanges();
                return Ok(problem);
            }
            return NotFound();
        }

        [HttpDelete("{changeId}/problems/{problemId}")]
        public IActionResult RemoveProblem(long changeId, long problemId)
        {
            var change = DbContext.Changes.Include(i => i.Problems).SingleOrDefault(i => i.Id == changeId);
            var problem = DbContext.Problems.Include(i => i.Changes).SingleOrDefault(i => i.Id == problemId);
            if (change != null && problem != null)
            {
                change.Problems.Remove(problem);
                problem.Changes.Remove(change);
                DbContext.SaveChanges();
                return Ok(problem);
            }
            return NotFound();
        }

        [HttpPatch("{changeId}/stateupdate")]
        public IActionResult StateUpdate([FromBody] string state, long changeId)
        {
            var change = DbContext.Changes.Include(i => i.ConfigurationItem).SingleOrDefault(i => i.Id == changeId);
            if (change != null)
            {
                change.State = state;
                if (state == State.IMPLEMENTADO)
                {
                    var item = change.ConfigurationItem;
                    var history = JsonConvert.DeserializeObject<Dictionary<string, object>>(item.VersionHistory);
                    var newVersionKey = "v1.0";
                    var highestVersionKey = "v0";
                    if (history != null)
                    {
                        highestVersionKey = history.Keys.Where(key => key.StartsWith("v"))
                        .Select(key => key.Substring(1))
                        .OrderByDescending(version => float.Parse(version, CultureInfo.InvariantCulture.NumberFormat))
                        .FirstOrDefault();
                        newVersionKey = IncrementVersion(highestVersionKey);
                    }
                    if (history == null) { history = new Dictionary<string, object>(); }
                    var newDescription = item.Description + $". Modified according to {change.Title} change. Update from {highestVersionKey} to {newVersionKey}";
                    history[newVersionKey] = String.Format("Titulo:{0}|Descripcion:{1}", item.Title, newDescription);
                    item.VersionId = newVersionKey;
                    item.VersionHistory = JsonConvert.SerializeObject(history);
                    item.Description = newDescription;
                }
                DbContext.Changes.Update(change);
                DbContext.SaveChanges();
                return Ok(change);
            }
            return BadRequest();
        }
    }
}
