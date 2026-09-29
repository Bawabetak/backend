
namespace bawabetak_backend.Extensions.Configurations;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            options.LogTo(Console.WriteLine, LogLevel.Information)
                   .EnableSensitiveDataLogging();
        });

        services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<Context>() 
        .AddDefaultTokenProviders();

        return services;
    }
}