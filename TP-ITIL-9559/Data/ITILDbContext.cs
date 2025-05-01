using Microsoft.EntityFrameworkCore;

namespace TP_ITIL_9559.Data
{
    public class ITILDbContext : DbContext
    {
        public ITILDbContext(DbContextOptions<ITILDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }
        }
    }
}
