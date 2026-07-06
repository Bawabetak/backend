using bawabetak_backend.Dtos;

namespace bawabetak_backend.Extensions.DependencyInjection
{
    public static class OptionsDI
    {
        public static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MailSettings>(config.GetSection("MailSettings"));
            

            services.AddMemoryCache();

            return services;
        }
    }
}
