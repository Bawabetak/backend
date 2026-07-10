
public static class AuthenticationConfiguration
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = config["JWT:Issuer"],
                ValidAudience = config["JWT:Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config["JWT:SecretKey"]!)),

                ValidateLifetime = true,

        
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    Console.WriteLine("===== OnMessageReceived =====");
                    Console.WriteLine("Request Path: " + path);
                    Console.WriteLine("Access Token from Query: " + accessToken);

                    if (string.IsNullOrEmpty(accessToken) && context.Request.Headers.ContainsKey("Authorization"))
                    {
                        accessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                        Console.WriteLine("Access Token from Header: " + accessToken);
                    }

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                        Console.WriteLine("Token assigned to context.Token ✔");
                    }

                    return Task.CompletedTask;
                },

                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("===== Authentication Failed =====");
                    Console.WriteLine("Exception: " + context.Exception.Message);
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}