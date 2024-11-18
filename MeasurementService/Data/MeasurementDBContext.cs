using Microsoft.EntityFrameworkCore;
using MeasurementService.Models;

namespace MeasurementService.Data
{
    public class MeasurementDbContext(DbContextOptions<MeasurementDbContext> options) : DbContext(options)
    {
        public DbSet<Measurement> Measurements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Measurement>().ToTable("Measurements");
        }
    }
}