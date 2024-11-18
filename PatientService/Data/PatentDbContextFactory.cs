using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PatientService.Data
{
    public class PatientDbContextFactory : IDesignTimeDbContextFactory<PatientDbContext>
    {
        public PatientDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("PatientDatabase")
                                   ?? throw new InvalidOperationException("Connection string 'PatientDatabase' not found.");

            var optionsBuilder = new DbContextOptionsBuilder<PatientDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new PatientDbContext(optionsBuilder.Options);
        }
    }
}