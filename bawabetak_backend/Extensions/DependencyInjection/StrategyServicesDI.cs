


namespace bawabetak_backend.Extensions.DependencyInjection
{
    public static class StrategyServicesDI
    {
        public static IServiceCollection AddVerificationStrategies(this IServiceCollection services)
        {
          

            services.AddScoped<ISendVerificationStrategy, RegisterSendStrategy>();
            services.AddScoped<ISendVerificationStrategy, ForgetPasswordSendStrategy>();
            services.AddScoped<ISendVerificationFactory, SendVerificationFactory>();
            services.AddScoped<ICheckVerificationStrategy, RegisterCheckStrategy>();
            services.AddScoped<ICheckVerificationStrategy, ForgetPasswordCheckStrategy>();
            services.AddScoped<ICheckVerificationFactory, CheckVerificationFactory>();
            services.AddScoped<ILoginFactory, LoginFactory>();
            services.AddScoped<ILoginStrategy, MobileLoginStrategy>();
            services.AddScoped<ILoginStrategy, WebLoginStrategy>();
            services.AddScoped<IRefreshTokenCoreService, RefreshTokenCoreService>();
            services.AddScoped<IRefreshTokenFactory, RefreshTokenFactory>();
            services.AddScoped<IRefreshTokenStrategy, MobileRefreshTokenStrategy>();
            services.AddScoped<IRefreshTokenStrategy, WebRefreshTokenStrategy>();


            return services;
        }
    }
}
