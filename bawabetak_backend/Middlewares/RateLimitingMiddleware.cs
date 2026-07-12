

namespace bawabetak_backend.Middlewares;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RateLimitingSettings _settings;

    public RateLimitingMiddleware(
        RequestDelegate next,
        IOptions<RateLimitingSettings> options)
    {
        _next = next;
        _settings = options.Value;
    }

    public async Task InvokeAsync(HttpContext context, ICacheHelper cacheHelper)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        if (ipAddress is "::1" or "127.0.0.1" or "unknown")
        {
            await _next(context);
            return;
        }

        var policy = _settings.GetPolicy(context.Request.Path.Value ?? "");

        string blockKey = $"Blocked_IP:{ipAddress}:{context.Request.Path}";
        string requestKey = $"Track_IP:{ipAddress}:{context.Request.Path}";

        var isBlocked = await cacheHelper.GetAsync<bool?>(blockKey);

        if (isBlocked == true)
        {
            await ReturnBlockedResponse(context);
            return;
        }

        var currentRequests = await cacheHelper.GetAsync<int?>(requestKey) ?? 0;

        if (currentRequests >= policy.MaxRequests)
        {
            await cacheHelper.SetAsync(
                blockKey,
                true,
                TimeSpan.FromMinutes(policy.BlockMinutes));

            await ReturnBlockedResponse(context);
            return;
        }

        currentRequests++;

        var ttl = currentRequests == 1
            ? TimeSpan.FromSeconds(policy.WindowSeconds)
            : (TimeSpan?)null;

        await cacheHelper.SetAsync(
            requestKey,
            currentRequests,
            ttl);

        await _next(context);
    }

    private static async Task ReturnBlockedResponse(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.Response.ContentType = "application/json";

        var response = ResponseHelper.Error(ResponseKeys.TooManyRequests);

        await context.Response.WriteAsync(JsonSerializer.Serialize(
            response,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
    }
}