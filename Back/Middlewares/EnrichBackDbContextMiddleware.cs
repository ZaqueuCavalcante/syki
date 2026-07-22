using System.Diagnostics;

namespace Estud.Back.Middlewares;

public class EnrichBackDbContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext request, EstudDbContext ctx)
    {
        if (request.User.IsAuthenticated)
        {
            ctx.RequestUser.Id = request.User.Id;
            ctx.RequestUser.Permissions = request.User.Permissions;
            ctx.RequestUser.InstitutionId = request.User.InstitutionId;
        }

        ctx.Operation = request.GetTargetControllerName();
        ctx.ActivityId = Activity.Current?.Id ?? Guid.CreateVersion7().ToString();

        await next(request);
    }
}
