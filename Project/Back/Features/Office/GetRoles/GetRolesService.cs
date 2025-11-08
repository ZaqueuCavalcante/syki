using Dapper;
using Exato.Shared.Features.Office.GetRoles;
using Exato.Back.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Office.GetRoles;

public class GetRolesService(BackDbContext ctx) : IOfficeService
{
    public async Task<GetRolesOut> Get(GetRolesIn data)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string totalSql = @"
            SELECT
                count(1)
            FROM
                exato.roles r
            INNER JOIN
                public.cliente c ON c.cliente_id = r.organization_id
            WHERE
                (@Name IS NULL OR r.name ILIKE @Name)
        ";

        const string itemsSql = @"
            SELECT
                r.id,
                r.name,
                r.features,
                r.description,
                r.organization_id,
                c.nome AS organization
            FROM
                exato.roles r
            INNER JOIN
                public.cliente c ON c.cliente_id = r.organization_id
            WHERE
                (@Name IS NULL OR r.name ILIKE @Name)
            ORDER BY
                r.name DESC
            LIMIT 10
            OFFSET @Offset
        ";

        var parameters = new
        {
            Offset = data.Page * 10,
            Name = data.Name.HasValue() ? $"%{data.Name.Trim()}%" : null,
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var items = await connection.QueryAsync<GetRolesItemOut>(itemsSql, parameters);

        return new GetRolesOut()
        {
            Total = total,
            Items = items.ToList(),
        };
    }
}
