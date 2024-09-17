namespace Syki.Back.Exceptions;

public class CustomHeadersMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-SykiHash"] = Env.GetLastCommitHash();
            context.Response.Headers["X-Testando"] = "0123456789";
            return Task.CompletedTask;
        });

        await next(context);
    }
}
