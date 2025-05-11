using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TP_ITIL_9559.Data;
using TP_ITIL_9559.Model;
using TP_ITIL_9559.Data.Domain;

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
                DbContext.Configuration.Add(new ConfigurationItem()
                {
                    Title = item.title,
                    Description = item.description,
                    CreatedDate = DateTime.UtcNow,
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
    }
}
