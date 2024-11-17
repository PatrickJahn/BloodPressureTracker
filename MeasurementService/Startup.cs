using Microsoft.EntityFrameworkCore;
using MeasurementService.Data;
using MeasurementService.Mapper;
using MeasurementService.Repositories;
using MeasurementService.Repositories.Interfaces;
using MeasurementService.Services.Interfaces;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;


namespace MeasurementService
{
    public class Startup(IConfiguration configuration)
    {
        private IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString =
                Configuration.GetConnectionString("MeasurementDatabase")
                ?? throw new InvalidOperationException("Connection string"
                                                       + "'DefaultConnection' not found.");

        
            services.AddDbContext<MeasurementDbContext>(options =>
                options.UseSqlServer(connectionString));
            
            services.AddAutoMapper(typeof(MappingProfile));
            
            services.AddScoped<IMeasurementRepository, MeasurementRepository>();
            services.AddScoped<IMeasurementService, Services.MeasurementService>();
          
            services.AddFeatureManagement();
            services.AddControllers();
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Measurements API", Version = "v1" });
            });        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
     
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeasurementService API v1"));


            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MeasurementDbContext>();
                try
                {
                    dbContext.Database.Migrate(); 
                    DBSeeder.Seed(dbContext);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error applying database migrations: {ex.Message}");
                    throw;
                }
            }

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