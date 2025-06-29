using Microsoft.EntityFrameworkCore;

namespace H2.Infrastructure.Persistence.AppDbContext
{
    public class H2DbContext : DbContext
    {
        public H2DbContext(DbContextOptions<H2DbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
        
        public DbSet<Domain.Entities.Device> Devices { get; set; }
        public DbSet<Domain.Entities.Alert> Alerts { get; set; }
        public DbSet<Domain.Entities.ThermalImage> ThermalImages { get; set; }
        public DbSet<Domain.Entities.SensorData> SensorData { get; set; }
    }
}
