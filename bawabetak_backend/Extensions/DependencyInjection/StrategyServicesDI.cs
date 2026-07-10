
using bawabetak_backend.Strategies.CheckVerification.Implementation;

namespace bawabetak_backend.Extensions.DependencyInjection
{
    public static class StrategyServicesDI
    {
        public static IServiceCollection AddVerificationStrategies(this IServiceCollection services)
        {
          

            services.AddScoped<ISendVerificationStrategy, RegisterSendStrategy>();
            services.AddScoped<ISendVerificationStrategy, ResetPasswordSendStrategy>();
            services.AddScoped<ISendVerificationFactory, SendVerificationFactory>();
            services.AddScoped<ICheckVerificationStrategy, RegisterCheckStrategy>();
            services.AddScoped<ICheckVerificationStrategy, ResetPasswordCheckStrategy>();
            services.AddScoped<ICheckVerificationFactory, CheckVerificationFactory>();



            return services;
        }
    }
}
