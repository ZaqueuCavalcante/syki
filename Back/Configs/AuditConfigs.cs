using Audit.Core;
using Syki.Back.Audit;
using Syki.Back.Features.Academic.CreateCampus;

namespace Syki.Back.Configs;

public static class AuditConfigs
{
    public static void AddAuditConfigs(this WebApplicationBuilder _)
    {
        Configuration.Setup().UseEntityFramework(_ => _
            .AuditTypeExplicitMapper(_ => _
                .Map<Campus, AuditLog>()
                .AuditEntityAction<AuditLog>((evt, entry, log) => log.Fill(evt, entry)))
            .IgnoreMatchedProperties(true));
    }

    public static void UseAudit(this IApplicationBuilder app)
    {
        var contextAccessor = app.ApplicationServices.GetService<IHttpContextAccessor>()!;

        Configuration.AddCustomAction(ActionType.OnScopeCreated, scope =>
        {
            var httpContext = contextAccessor.HttpContext;
            if (httpContext == null) return;

            if (!httpContext.User.IsAuthenticated)
            {
                scope.Event.CustomFields["Skip"] = true;
                return;
            }

            scope.Event.CustomFields["UserId"] = httpContext.User.Id;
            scope.Event.CustomFields["InstitutionId"] = httpContext.User.InstitutionId;
        });
    }
}
