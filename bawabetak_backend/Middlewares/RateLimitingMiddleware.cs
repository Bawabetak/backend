

namespace bawabetak_backend.Middlewares;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;

    private const int MaxRequests = 10;
    private const int WindowSeconds = 10;
    private const int BlockMinutes = 30;

    public RateLimitingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICacheHelper cacheHelper)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        if (ipAddress == "::1" || ipAddress == "127.0.0.1" || ipAddress == "unknown")
        {
            await _next(context);
            return;
        }

        string blockKey = $"Blocked_IP:{ipAddress}";
        string requestKey = $"Track_IP:{ipAddress}";

        var isBlocked = await cacheHelper.GetAsync<bool?>(blockKey);
        if (isBlocked == true)
        {
            await ReturnBlockedResponse(context);
            return;
        }

        var currentRequests = await cacheHelper.GetAsync<int?>(requestKey) ?? 0;

        if (currentRequests >= MaxRequests)
        {
            await cacheHelper.SetAsync(blockKey, true, TimeSpan.FromMinutes(BlockMinutes));

            await ReturnBlockedResponse(context);
            return;
        }

        currentRequests++;
        var ttl = currentRequests == 1 ? TimeSpan.FromSeconds(WindowSeconds) : (TimeSpan?)null;
        await cacheHelper.SetAsync(requestKey, currentRequests, ttl);

        await _next(context);
    }

    private async Task ReturnBlockedResponse(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;

        var apiResponse = ResponseHelper.Error(ResponseKeys.TooManyRequests);
        var jsonResponse = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}