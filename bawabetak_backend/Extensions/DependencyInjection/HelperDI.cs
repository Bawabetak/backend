using Microsoft.AspNetCore.Identity;
using Wasla_Backend.Helpers.EmailSender;

namespace bawabetak_backend.Extensions.DependencyInjection
{
    public static class HelperDI
    {
        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {

            services.AddScoped<IEmailSenderHelper, EmailSenderHelper>();
            return services;
        }
    }
}
