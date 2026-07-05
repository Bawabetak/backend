

namespace bawabetak_backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
               .AddDatabase(builder.Configuration)
               .AddSwaggerConfiguration()
               .AddControllers();

        var app = builder.Build();
    

        app.UseApplicationPipeline();

        app.Run();
    }
}