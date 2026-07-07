
namespace bawabetak_backend.Helpers.ProgramHelper.DependencyInjection
{
    public static class ServicesDI
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                 .FromAssemblies(
                     typeof(IFileService).Assembly
                 )
                 .AddClasses(c => c.Where(t => t.Name.EndsWith("Service")))
                 .AsImplementedInterfaces()
                 .WithScopedLifetime()
             );

            services.AddHttpContextAccessor();

            return services;
        }
    }
}