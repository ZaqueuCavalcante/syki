using System.Diagnostics;

namespace Exato.Back.Middlewares;

public class EnrichBackDbContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, BackDbContext ctx)
    {
        if (context.User.IsAuthenticated)
        {
            ctx.UserId = context.User.Id;
            ctx.OrganizationId = context.User.OrganizationId;
        }

        ctx.ActivityId = Activity.Current?.Id ?? Guid.CreateVersion7().ToString();
        ctx.Operation = context.GetTargetControllerName();

        await next(context);
    }
}
