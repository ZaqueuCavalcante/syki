using Audit.Core;
using Exato.Back.Audit;
using Audit.EntityFramework;
using Exato.Back.Intelligence.Domain.Public;
using AuditConfig = Audit.Core.Configuration;

namespace Exato.Back.Configs;

public static class AuditConfigs
{
    public static void AddAuditConfigs(this WebApplicationBuilder _)
    {
        AuditConfig.Setup().UseEntityFramework(_ => _
            .AuditTypeExplicitMapper(_ => _
                .Map<User, AuditTrail>()
                .Map<Cliente, AuditTrail>()
                .Map<TokenAcesso, AuditTrail>()
                .Map<OrganizationUser, AuditTrail>()
                .AuditEntityAction<AuditTrail>((evt, entry, trail) =>
                {
                    // To fix audit infinity cycle in case of DbUpdateException
                    if (evt.Environment.Exception != null) return false;

                    return trail.Fill(evt, entry);
                }))
            .IgnoreMatchedProperties(true));

        AuditConfig.AddCustomAction(ActionType.OnScopeCreated, scope =>
        {
            var ctx = scope.GetEntityFrameworkEvent().GetDbContext() as BackDbContext;
            scope.SetUserId(ctx.UserId);
            scope.SetOrganizationId(ctx.OrganizationId);
            scope.SetActivityId(ctx.ActivityId);
            scope.SetOperation(ctx.Operation);
        });
    }
}
