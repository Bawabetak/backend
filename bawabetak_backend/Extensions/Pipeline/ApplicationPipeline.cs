namespace bawabetak_backend.Extensions.Pipeline;

public static class ApplicationPipeline
{
    
        public static WebApplication UseApplicationPipeline(this WebApplication app)
        {


        app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();
            app.UseCors("AllowAll"); 

            app.UseMiddleware<RateLimitingMiddleware>();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                if (!app.Environment.IsDevelopment())
                {
                    options.RoutePrefix = string.Empty;
                }
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Bawabetak API V1");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new AllowAllDashboardAuthorizationFilter() }
            });

        RecurringJob.AddOrUpdate<IRemoveUnVerifiedEmailsJob>("Remove inValidEmails"
            , job => job.RemoveUnVerifiedEmails(),
            Cron.MinuteInterval(15));

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