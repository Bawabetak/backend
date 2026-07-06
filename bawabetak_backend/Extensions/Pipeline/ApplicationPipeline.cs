
namespace bawabetak_backend.Extensions.Pipeline;

public static class ApplicationPipeline
{
    public static WebApplication UseApplicationPipeline(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<RateLimitingMiddleware>();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            if (!app.Environment.IsDevelopment())
            {
                options.RoutePrefix = string.Empty;
            }
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Bawabetak API V1");
        });


        app.UseAuthorization();

        app.MapControllers();
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new AllowAllDashboardAuthorizationFilter() }
        });
        return app;
    }
    public class AllowAllDashboardAuthorizationFilter : Hangfire.Dashboard.IDashboardAuthorizationFilter
    {
        public bool Authorize(Hangfire.Dashboard.DashboardContext context)
        {
            return true;
        }
    }
}