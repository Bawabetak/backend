

namespace bawabetak_backend.Extensions.DependencyInjection
{
    public static class JobDI
    {
        public static IServiceCollection AddJobs(this IServiceCollection services)
        {
            services.AddScoped<IRegisterVerificationJob, RegisterVerificationJob>();
            return services;
        }
    }
}
