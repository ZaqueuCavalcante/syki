using Dapper;
using Npgsql;

namespace Syki.Back.Features.Cross.GetUserAccount;

public class GetUserAccountService(DatabaseSettings settings) : ICrossService
{
    public async Task<GetUserAccountOut> Get(Guid userId)
    {
        await using var dataSource = NpgsqlDataSource.Create(settings.ConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT
                u.name,
                u.email,
                u.profile_photo,
                r.name AS role,
                i.name AS institution
            FROM
                syki.users u
            INNER JOIN
                syki.user_roles ur ON ur.user_id = u.id
            INNER JOIN
                syki.roles r ON r.id = ur.role_id
            INNER JOIN
                syki.institutions i ON i.id = u.institution_id
            WHERE
                u.id = @UserId
            LIMIT 1
        ";

        return await connection.QueryFirstAsync<GetUserAccountOut>(sql, new { userId });
    }
}
