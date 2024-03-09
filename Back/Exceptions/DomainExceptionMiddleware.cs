using System.Text.Json;

namespace Syki.Back.Exceptions;

public class DomainExceptionMiddleware(RequestDelegate next)
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

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var result = JsonSerializer.Serialize(new ErrorOut { Message = ex.Message });

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = 500;

        return context.Response.WriteAsync(result);
    }
}
