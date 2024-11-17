using Microsoft.FeatureManagement;

namespace APIGateway.Middleware
{
    public static class FeatureFlagMiddlewareExtensions
    {
        public static void UseFeatureFlagMiddleware(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var featureManager = context.RequestServices.GetRequiredService<IFeatureManager>();

                if (await IsRouteDisabled(featureManager, context))
                {
                    context.Response.StatusCode = 503;
                    await context.Response.WriteAsync("The requested feature is currently disabled.");
                    return;
                }

                await next();
            });
        }

        private static async Task<bool> IsRouteDisabled(IFeatureManager featureManager, HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/patients") &&
                !await featureManager.IsEnabledAsync("EnablePatientRoutes"))
            {
                return true;
            }

            if (context.Request.Path.StartsWithSegments("/measurements") &&
                !await featureManager.IsEnabledAsync("EnableMeasurementRoutes"))
            {
                return true;
            }

            return false;
        }
    }
}