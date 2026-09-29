namespace bawabetak_backend.Extensions.Configurations
{
    public static class CorsPolicy
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.SetIsOriginAllowed(origin => true)
                           .AllowAnyMethod()
                           .AllowAnyHeader() 
                           .AllowCredentials(); 
                });
            });
            return services;
        }
    }
}