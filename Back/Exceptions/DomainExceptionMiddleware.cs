using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace Syki.Back.Exceptions;

public class DomainExceptionMiddleware(RequestDelegate next, ILogger logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            await HandleDomainExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleDomainExceptionAsync(HttpContext context, DomainException ex)
    {
        var result = JsonSerializer.Serialize(new ErrorOut { Message = ex.Message });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ex.StatusCode;

        return context.Response.WriteAsync(result);
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var result = JsonSerializer.Serialize(new ErrorOut { Message = "Internal Server Error" });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        logger.Error("Internal Server Error -> {Message}", ex.Message);

        return context.Response.WriteAsync(result);
    }
}
