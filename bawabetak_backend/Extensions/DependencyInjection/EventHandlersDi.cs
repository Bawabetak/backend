

namespace bawabetak_backend.Extensions.DependencyInjection
{
    public static class EventHandlersDi
    {
        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            services.AddScoped<IEventPublisher, EventPublisher>();

            services.AddScoped<
                IEventHandler<UserRegisteredEvent>,
                UserRegisteredEventHandler>();
            return services;
        }
    }
}