

namespace bawabetak_backend.Extensions.DependencyInjection
{
    public static class RepositoriesDI
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.Scan(scan => scan
      .FromAssembliesOf(typeof(GenericRepository<>))
      .AddClasses(c => c.Where(t => t.Name.EndsWith("Repository")))
      .AsImplementedInterfaces()
      .WithScopedLifetime()
  );

            return services;
        }
    }
}
