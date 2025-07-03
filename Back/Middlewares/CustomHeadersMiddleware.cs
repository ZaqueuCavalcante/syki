namespace Syki.Back.Middlewares;

public class CustomHeadersMiddleware(RequestDelegate next, FeaturesSettings settings)
{
    public async Task Invoke(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-DeployHash"] = Env.DeployHash;
            context.Response.Headers["X-CrossLogin"] = settings.CrossLogin.ToString();
            return Task.CompletedTask;
        });

        await next(context);
    }
}
