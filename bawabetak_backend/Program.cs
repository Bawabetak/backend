


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
               .AddServices()
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