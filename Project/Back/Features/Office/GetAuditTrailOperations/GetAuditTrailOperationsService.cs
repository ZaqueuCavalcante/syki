using Dapper;
using Exato.Back.Features.Cross.CreateExatoUser;
using Exato.Shared.Features.Office.GetAuditTrailOperations;

namespace Exato.Back.Features.Office.GetAuditTrailOperations;

public class GetAuditTrailOperationsService(BackDbContext ctx) : IOfficeService
{
    public async Task<GetAuditTrailOperationsOut> Get()
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string itemsSql = @"
            SELECT
                operation
            FROM
                exato.audit_trails
            GROUP BY
                operation
            ORDER BY
                operation
        ";

        var items = (await connection.QueryAsync<string>(itemsSql)).ToList();

        return new GetAuditTrailOperationsOut()
        {
            Items = items,
        };
    }
}
