using Dapper;
using Exato.Shared.Features.Office.GetUsers;
using Exato.Back.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Office.GetUsers;

public class GetUsersService(BackDbContext ctx) : IOfficeService
{
    public async Task<GetUsersOut> Get(GetUsersIn data)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string totalSql = @"
            SELECT
                count(1)
            FROM
                exato.users u
            INNER JOIN
                exato.user_roles ur ON ur.user_id = u.id
            INNER JOIN
                exato.roles r ON r.id = ur.role_id
            WHERE
                (@Name IS NULL OR u.name ILIKE @Name)
        ";

        const string itemsSql = @"
            SELECT
                u.id,
                u.name,
                u.email,
                u.created_at,
                r.name AS role,
                u.two_factor_enabled
            FROM
                exato.users u
            INNER JOIN
                exato.user_roles ur ON ur.user_id = u.id
            INNER JOIN
                exato.roles r ON r.id = ur.role_id
            WHERE
                (@Name IS NULL OR u.name ILIKE @Name)
            ORDER BY
                u.created_at DESC
            LIMIT 10
            OFFSET @Offset
        ";

        var parameters = new
        {
            Offset = data.Page * 10,
            Name = data.Name.HasValue() ? $"%{data.Name.Trim()}%" : null,
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var users = (await connection.QueryAsync<ExatoUser>(itemsSql, parameters)).ToList();

        return new GetUsersOut()
        {
            Total = total,
            Items = users.ConvertAll(x => x.ToGetUsersItemOut())
        };
    }
}
