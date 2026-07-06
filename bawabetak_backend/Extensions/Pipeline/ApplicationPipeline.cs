namespace bawabetak_backend.Extensions.Pipeline;

public static class ApplicationPipeline
{
    public static WebApplication UseApplicationPipeline(this WebApplication app)
    {
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

        return app;
    }
}