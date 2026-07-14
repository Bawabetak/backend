namespace bawabetak_backend.Extensions.Configurations
{
    public static class MediatRConfiguration
    {
        public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
            return services;
        }
    }
}
