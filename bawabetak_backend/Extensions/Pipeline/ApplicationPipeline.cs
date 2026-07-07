namespace bawabetak_backend.Extensions.Pipeline;

public static class ApplicationPipeline
{
    public static WebApplication UseApplicationPipeline(this WebApplication app)
    {
        // 1. أول حاجة الـ Exception عشان يلقط أي مشكلة بتحصل في الميدل ويرز اللي بعده
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<RateLimitingMiddleware>();

        // 2. تفعيل الـ Static Files عشان يقرا الفايلات من الـ wwwroot (سواء موجود لوكال أو على الهوست)
        // لازم يتحط بدري عشان لو جالك ريكويست على صورة يرجعها علطول من غير ما يضيع وقت في الـ Routing والـ Auth
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

        app.UseRouting();

     
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