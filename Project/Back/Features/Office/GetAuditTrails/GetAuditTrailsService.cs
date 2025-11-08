using Dapper;
using Exato.Back.Features.Cross.CreateExatoUser;
using Exato.Shared.Features.Office.GetAuditTrails;

namespace Exato.Back.Features.Office.GetAuditTrails;

public class GetAuditTrailsService(BackDbContext ctx) : IOfficeService
{
    public async Task<GetAuditTrailsOut> Get(GetAuditTrailsIn data)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string totalSql = @"
            SELECT
                count(1)
            FROM
                exato.audit_trails
            WHERE
                (@Operation IS NULL OR operation = @Operation)
                    AND
                (@Action IS NULL OR action = @Action)
                    AND
                (@UserId IS NULL OR user_id = @UserId)
        ";

        const string itemsSql = @"
            SELECT
                a.id,
                a.activity_id,
                a.operation,
                a.entity_type,
                a.user_id,
                u.name AS user,
                a.action,
                a.created_at
            FROM
                exato.audit_trails a
            LEFT JOIN
                exato.users u ON u.id = a.user_id
            WHERE
                (@Operation IS NULL OR operation = @Operation)
                    AND
                (@Action IS NULL OR a.action = @Action)
                    AND
                (@UserId IS NULL OR a.user_id = @UserId)
            ORDER BY
                a.created_at DESC
            LIMIT 10
            OFFSET @Offset
        ";

        var parameters = new
        {
            data.Action,
            data.UserId,
            data.Operation,
            Offset = data.Page * 10,
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var items = (await connection.QueryAsync<GetAuditTrailsItemOut>(itemsSql, parameters)).ToList();

        return new GetAuditTrailsOut()
        {
            Total = total,
            Items = items,
        };
    }
}
