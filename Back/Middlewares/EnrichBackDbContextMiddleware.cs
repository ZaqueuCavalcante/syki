using System.Diagnostics;

namespace Syki.Back.Middlewares;

public class EnrichBackDbContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext request, SykiDbContext ctx)
    {
        if (request.User.IsAuthenticated)
        {
            ctx.RequestUser.RoleId = request.User.RoleId;
            ctx.RequestUser.Permissions = request.User.Permissions;

            ctx.RequestUser.Id = request.User.Id;
            ctx.RequestUser.InstitutionId = request.User.InstitutionId;
        }

        ctx.ActivityId = Activity.Current?.Id ?? Guid.CreateVersion7().ToString();
        ctx.Operation = request.GetTargetControllerName();

        await next(request);
    }
}
