using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.Logging;
using PatientService.Repositories;
using PatientService.Repositories.Interfaces;
using PatientService.Services.Interfaces;
using PatientService.Utilities;

namespace PatientService
{
    public class Startup(IConfiguration configuration)
    {
        private IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString =
                Configuration.GetConnectionString("PatientDatabase")
                ?? throw new InvalidOperationException("Connection string"
                                                       + "'DefaultConnection' not found.");

        
            services.AddDbContext<PatientDbContext>(options =>
                options.UseSqlServer(connectionString));
            
            services.AddAutoMapper(typeof(Startup));
            
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPatientService, Services.PatientService>();
            services.AddLoggingConfiguration(Configuration);
            
            services.AddControllers();
            services.AddSwaggerGen();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PatientService API v1"));
            }
            app.UseMiddleware<ErrorHandler>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
