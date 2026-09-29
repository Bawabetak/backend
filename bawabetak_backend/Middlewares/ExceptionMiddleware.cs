
namespace bawabetak_backend.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        ApiResponse apiResponse;

        if (exception is BaseException baseEx)
        {
            context.Response.StatusCode = baseEx.StatusCode; 
            apiResponse = ResponseHelper.Error(baseEx.ResponseKey);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            dynamic? errorData = _env.IsDevelopment() ? exception.StackTrace : null;
            apiResponse = ResponseHelper.Error(ResponseKeys.InternalServerError, errorData);
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var jsonResponse = JsonSerializer.Serialize(apiResponse, jsonOptions);

        await context.Response.WriteAsync(jsonResponse);
    }
}