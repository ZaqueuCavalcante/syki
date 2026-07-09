using Audit.Core;
using Estud.Back.Audit;
using Audit.EntityFramework;
using Estud.Back.Domain.Identity;
using AuditConfig = Audit.Core.Configuration;

namespace Estud.Back.Configs;

public static class AuditConfigs
{
    public static void AddAuditConfigs(this WebApplicationBuilder _)
    {
        AuditConfig.Setup().UseEntityFramework(_ => _
            .AuditTypeExplicitMapper(_ => _
                .Map<EstudUser, AuditTrail>()
                .Map<MagicLink, AuditTrail>()
                .AuditEntityAction<AuditTrail>((evt, entry, trail) =>
                {
                    if (evt.Environment.Exception != null) return false;
                    return trail.Fill(evt, entry);
                }))
            .IgnoreMatchedProperties(true));

        AuditConfig.AddCustomAction(ActionType.OnScopeCreated, scope =>
        {
            var dbContext = scope.GetEntityFrameworkEvent().GetDbContext() as EstudDbContext;

            scope.SetUserId(dbContext.RequestUser.Id);
            scope.SetInstitutionId(dbContext.RequestUser.InstitutionId);
            scope.SetActivityId(dbContext.ActivityId);
            scope.SetOperation(dbContext.Operation);
        });
    }
}
