

using Bawabetak.Extensions.Configurations;
using bawabetak_backend.Extensions.DependencyInjection;

namespace bawabetak_backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
               .AddDatabase(builder.Configuration)
               .AddSwaggerConfiguration()
               .AddRepositories()
               .AddAppOptions(builder.Configuration)
               .AddHelpers()
               .AddCacheServices(builder.Configuration)
               .AddHangfireServices(builder.Configuration)
               .AddAutoMapperConfiguration()
               .AddControllers();               

        var app = builder.Build();
    

        app.UseApplicationPipeline();

        app.Run();
    }
}