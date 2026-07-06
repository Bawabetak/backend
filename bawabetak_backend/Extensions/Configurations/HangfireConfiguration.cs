
namespace Bawabetak.Extensions.Configurations;

public static class HangfireConfiguration
{
    public static IServiceCollection AddHangfireServices(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("RedisConnection");

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseRedisStorage(redisConnectionString, new RedisStorageOptions
            {
                Prefix = "Bawabetak_Hangfire:" 
            }));

        services.AddHangfireServer();

        return services;
    }
}