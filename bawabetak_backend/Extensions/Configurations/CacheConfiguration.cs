
namespace bawabetak_backend.Extensions.Configurations;

public static class CacheConfiguration
{
    public static IServiceCollection AddCacheServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "Bawabetak_"; 
        });

    

        return services;
    }
}