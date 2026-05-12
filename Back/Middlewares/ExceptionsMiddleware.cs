using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace Syki.Back.Middlewares;

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
        var message = EnvironmentExtensions.IsDevelopmentOrTesting() ? GetFullExceptionMessage(ex) : "Erro ao executar essa ação.";
        var result = JsonSerializer.Serialize(new ErrorOut { Code = "Error", Message = message });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        logger.Error("Internal Server Error -> {Message}", message);

        return context.Response.WriteAsync(result);
    }

    private static string GetFullExceptionMessage(Exception ex)
    {
        var message = ex.Message;
        if (ex.InnerException is not null)
        {
            message += " --> " + GetFullExceptionMessage(ex.InnerException);
        }
        return message;
    }
}
