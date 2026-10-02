using ProductMaintenance.Exceptions;

namespace ProductMaintenance.Middleware;

public class ExceptionHandling
{
    private readonly ILogger<ExceptionHandling> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
    {
        _next = next;
        _logger = logger;
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.Clear();
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
            {
                NotFoundException =>
                    (StatusCodes.Status404NotFound, exception.Message),

                ConflictException =>
                    (StatusCodes.Status409Conflict, exception.Message),

                _ =>
                    (
                        StatusCodes.Status500InternalServerError,
                        "An unexpected error occurred."
                    )
            };

        context.Response.StatusCode = statusCode;

        var response = new
        {
            success = false,
            statusCode,
            message,
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsJsonAsync(response);
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Unhandled exception processing {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await HandleExceptionAsync(context, exception);
        }
    }
}
