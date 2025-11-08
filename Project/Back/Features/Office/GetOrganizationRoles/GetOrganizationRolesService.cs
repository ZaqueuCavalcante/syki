using Dapper;
using Exato.Back.Features.Cross.CreateExatoUser;
using Exato.Shared.Features.Office.GetOrganizationRoles;

namespace Exato.Back.Features.Office.GetOrganizationRoles;

public class GetOrganizationRolesService(BackDbContext ctx) : IOfficeService
{
    public async Task<GetOrganizationRolesOut> Get(int id, GetOrganizationRolesIn data)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string totalSql = @"
            SELECT
                count(1)
            FROM
                exato.roles r
            WHERE
                r.organization_id = @Id
                    AND
                (@Name IS NULL OR r.name ILIKE @Name)
        ";

        const string itemsSql = @"
            SELECT
                r.id,
                r.name
            FROM
                exato.roles r
            WHERE
                r.organization_id = @Id
                    AND
                (@Name IS NULL OR r.name ILIKE @Name)
            ORDER BY
                r.name DESC
            LIMIT 10
            OFFSET @Offset
        ";

        var parameters = new
        {
            id,
            Offset = data.Page * 10,
            Name = data.Name.HasValue() ? $"%{data.Name.Trim()}%" : null,
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var items = await connection.QueryAsync<GetOrganizationRolesItemOut>(itemsSql, parameters);

        return new GetOrganizationRolesOut()
        {
            Total = total,
            Items = items.ToList(),
        };
    }
}
