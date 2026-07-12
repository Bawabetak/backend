
namespace bawabetak_backend.Extensions.DependencyInjection
{
    public static class OptionsDI
    {
        public static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MailSettings>(config.GetSection("MailSettings"));
            services.Configure<JwtSettings>(config.GetSection("Jwt"));
            services.Configure<RateLimitingSettings>(config.GetSection("RateLimiting"));    


            services.AddMemoryCache();

            return services;
        }
    }
}
