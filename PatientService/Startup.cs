using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using PatientService.Data;
using PatientService.Logging;
using PatientService.Mappers;
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

            services.AddAutoMapper(typeof(PatientMappingProfile));
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPatientService, Services.PatientService>();
            services.AddLoggingConfiguration(Configuration);
            services.AddFeatureManagement();
            services.AddControllers();
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Patients API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PatientService API v1"));

            app.UseMiddleware<ErrorHandler>();

            ApplyMigrations(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ApplyMigrations(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PatientDbContext>();

            try
            {
                dbContext.Database.Migrate(); 
                DbSeeder.Seed(dbContext);  
                Console.WriteLine("Successfully applied migrations and seeded the database.");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL error during migration: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying migrations: {ex.Message}");
                throw; 
            }
        }
    }
}
