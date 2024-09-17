namespace Syki.Back.Exceptions;

public class CustomHeadersMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-Hash"] = Env.GetLastCommitHash();
            return Task.CompletedTask;
        });

        await next(context);
    }
}
