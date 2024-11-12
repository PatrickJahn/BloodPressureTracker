using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace PatientService.Logging
{
    public static class LoggingConfig
    {
        public static void AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
           
        }
    }
}