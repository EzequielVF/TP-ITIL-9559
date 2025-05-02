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
    }
}
