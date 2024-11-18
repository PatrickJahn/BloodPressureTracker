using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MeasurementService.Data
{
    public class MeasurementDbContextFactory : IDesignTimeDbContextFactory<MeasurementDbContext>
    {
        public MeasurementDbContext CreateDbContext(string[] args)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables() 
                .Build();

            var connectionString = configuration.GetConnectionString("MeasurementDatabase")
                                   ?? throw new InvalidOperationException("Connection string 'MeasurementDatabase' not found.");

            var optionsBuilder = new DbContextOptionsBuilder<MeasurementDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new MeasurementDbContext(optionsBuilder.Options);
        }
    }
}