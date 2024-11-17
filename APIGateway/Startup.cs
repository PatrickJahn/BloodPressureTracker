using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.FeatureManagement;
using APIGateway.Middleware;
using Ocelot.Provider.Polly;

namespace APIGateway
{
    public class Startup(IConfiguration configuration)
    {
        private IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot().AddPolly();
            services.AddSwaggerForOcelot(Configuration);
            services.AddMvc();
            services.AddFeatureManagement();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Configure Swagger for Ocelot UI
            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.DownstreamSwaggerEndPointBasePath = "/swagger/docs";
                opt.PathToSwaggerGenerator = "/swagger/docs";
            });

            // Add Feature Flag Middleware
            app.UseFeatureFlagMiddleware();

            // Use Ocelot Middleware
            app.UseOcelot().Wait();
        }

    }
}