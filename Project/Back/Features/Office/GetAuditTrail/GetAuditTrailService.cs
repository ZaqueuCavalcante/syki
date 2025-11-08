using System.Text.Json;
using Exato.Back.Audit;
using Exato.Back.Features.Cross.CreateExatoUser;
using Exato.Shared.Features.Office.GetAuditTrail;

namespace Exato.Back.Features.Office.GetAuditTrail;

public class GetAuditTrailService(BackDbContext ctx) : IOfficeService
{
    public async Task<OneOf<GetAuditTrailOut, ExatoError>> Get(Guid id)
    {
        var trail = await ctx.ExatoAuditTrails.AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if (trail == null) return AuditTrailNotFound.I;

        var user = await ctx.Users.Where(x => x.Id == trail.UserId)
            .Select(x => x.Name).FirstOrDefaultAsync();

        var data = trail.Data.Deserialize<AuditData>();

        return new GetAuditTrailOut()
        {
            Name = data.Name,
            User = user ?? "-",
            Table = data.Table,
            Schema = data.Schema,
            EntityId = trail.EntityId,
            Action = trail.Action,
            Operation = trail.Operation,
            CreatedAt = trail.CreatedAt,
            Values = data.Values?.Select(x => new AuditValueOut { Column = x.Key, Value = x.Value }).ToList() ?? [],
            Changes = data.Changes?.ConvertAll(x => new AuditChangeOut { Column = x.Column, Old = x.Old, New = x.New }) ?? [],
        };
    }
}
