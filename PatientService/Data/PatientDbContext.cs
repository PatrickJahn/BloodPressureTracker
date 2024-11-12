using Microsoft.EntityFrameworkCore;
using PatientService.Models;

namespace PatientService.Data
{
    public class PatientDbContext(DbContextOptions<PatientDbContext> options) : DbContext(options)
    {
        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}