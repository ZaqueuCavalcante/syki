using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace Exato.Back.Middlewares;

public class ExceptionsMiddleware(RequestDelegate next, ILogger logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var message = EnvironmentExtensions.IsDevelopmentOrTesting() ? ex.Message : "Internal Server Error";
        var result = JsonSerializer.Serialize(new ErrorOut { Message = message });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        logger.Error("Internal Server Error -> {Message}", ex.Message);

        return context.Response.WriteAsync(result);
    }
}
