namespace Syki.Back.Middlewares;

public class UserDataMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, SykiDbContext ctx)
    {
        if (context.User.IsAuthenticated)
        {
            ctx.UserId = context.User.Id;
            ctx.InstitutionId = context.User.InstitutionId;
        }

        await next(context);
    }
}
