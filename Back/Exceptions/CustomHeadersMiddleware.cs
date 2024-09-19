namespace Syki.Back.Exceptions;

public class CustomHeadersMiddleware(RequestDelegate next, FeaturesSettings settings)
{
    public async Task Invoke(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-Hash"] = Env.GetLastCommitHash();
            context.Response.Headers["X-SkipUserRegister"] = settings.SkipUserRegister.ToString();
            return Task.CompletedTask;
        });

        await next(context);
    }
}
