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
            var root = GetRootException(ex);
            _logger.LogError(ex, "Unhandled exception. Root cause: {RootMessage}", root.Message);
            await HandleExceptionAsync(context, ex, root);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, Exception root)
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

            // التفاصيل تظهر في Development بس، في Production مبنرجعش حاجة
            dynamic? errorData = _env.IsDevelopment()
                ? new
                {
                    message = root.Message,
                    type = root.GetType().Name,
                    stackTrace = exception.StackTrace
                }
                : null;

            apiResponse = ResponseHelper.Error(ResponseKeys.InternalServerError, errorData);
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(apiResponse, jsonOptions));
    }

    private static Exception GetRootException(Exception ex)
    {
        while (ex.InnerException != null)
            ex = ex.InnerException;
        return ex;
    }
}