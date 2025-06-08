using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TP_ITIL_9559.Data;
using TP_ITIL_9559.Model;
using TP_ITIL_9559.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace TP_ITIL_9559.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        public ITILDbContext DbContext { get; set; }
        public ConfigurationController(ITILDbContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpPost("create")]
        public IActionResult SaveItem([FromBody] ConfigurationItemDto item)
        {
            if (ModelState.IsValid)
            {
                var user = DbContext.Users.SingleOrDefault(u => u.Id == item.userId);
                var history = new Dictionary<string, object>();
                history[item.versionId] = String.Format("Titulo:{0}|Descripcion:{1}", item.title, item.description);
                TimeZoneInfo argentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time");
                DateTime nowArgentina = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, argentinaTimeZone);
                DbContext.Configuration.Add(new ConfigurationItem()
                {
                    Title = item.title,
                    Description = item.description,
                    CreatedDate = nowArgentina,
                    UserId = item.userId,
                    User = user,
                    VersionId = item.versionId,
                    VersionHistory = JsonConvert.SerializeObject(history)
                });

                DbContext.SaveChanges();
                return Ok(item);
            }
            return BadRequest();
        }

        [HttpGet("items")]
        public IActionResult Items()
        {
            return Ok(DbContext.Configuration.OrderByDescending(i => i.CreatedDate));
        }

        [HttpGet("item/{itemId}")]
        public IActionResult Items(long itemId)
        {
            return Ok(DbContext.Configuration.SingleOrDefault(i => i.Id == itemId));
        }

        [HttpPatch("item/{itemId}")]
        public IActionResult UpdateItem([FromBody] ConfigurationItemDto modifiedItem, long itemId)
        {
            if (ModelState.IsValid)
            {
                var item = DbContext.Configuration.SingleOrDefault(i => i.Id == itemId);
                if (item != null)
                {
                    var history = JsonConvert.DeserializeObject<Dictionary<string, object>>(item.VersionHistory);
                    history[modifiedItem.versionId] = String.Format("Titulo:{0}|Descripcion:{1}", modifiedItem.title, modifiedItem.description);
                    item.Title = modifiedItem.title;
                    item.Description = modifiedItem.description;
                    item.VersionId = modifiedItem.versionId;
                    item.VersionHistory = JsonConvert.SerializeObject(history);
                    DbContext.Configuration.Update(item);
                    DbContext.SaveChanges();
                    return Ok(item);
                }
                return NotFound("Configuration Item not found!");
            }
            return BadRequest("Bad request!");
        }

        [HttpDelete("item/{itemId}")]
        public IActionResult DeleteItem(long itemId)
        {
            var item = DbContext.Configuration.SingleOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                DbContext.Configuration.Remove(item);
                DbContext.SaveChanges();
                return Ok($"Item: {itemId} deleted succesfuly");
            }
            return NotFound($"Item: {itemId} not found");
        }

        [HttpGet("item/{itemId}/incidents")]
        public IActionResult GetIncidentsByConfigurationItem(int itemId)
        {
            var configurationItem = DbContext.Configuration
                .Include(ci => ci.User)
                .SingleOrDefault(ci => ci.Id == itemId);

            if (configurationItem == null)
            {
                return NotFound($"ConfigurationItem with id {itemId} not found.");
            }

            var incidents = DbContext.Incidents
                .Include(i => i.User)
                .Include(i => i.AssignedUser)
                .Where(i => i.ConfigurationItemId == itemId)
                .OrderByDescending(i => i.CreatedDate)
                .ToList();

            return Ok(incidents);
        }
    }
}
