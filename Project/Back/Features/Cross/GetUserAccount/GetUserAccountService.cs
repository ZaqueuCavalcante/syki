using Npgsql;
using Dapper;
using Exato.Shared.Features.Cross.GetUserAccount;

namespace Exato.Back.Features.Cross.GetUserAccount;

public class GetUserAccountService(NpgsqlDataSource dataSource) : ICrossService
{
    public async Task<GetUserAccountOut> Get(Guid userId)
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = $@"
            SELECT
                u.id,
                u.name,
                u.email,
                r.features,
                r.name AS role,
                c.nome AS organization
            FROM
                exato.users u
            INNER JOIN
                exato.user_roles ur ON ur.user_id = u.id
            INNER JOIN
                exato.roles r ON r.id = ur.role_id
            INNER JOIN
                public.cliente c ON c.cliente_id = u.organization_id
            WHERE
                u.id = @UserId
            LIMIT 1
        ";

        return await connection.QueryFirstAsync<GetUserAccountOut>(sql, new { userId });
    }
}
