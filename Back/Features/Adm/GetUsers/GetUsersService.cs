using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetUsers;

public class GetUsersService(DatabaseSettings dbSettings) : IAdmService
{
    public async Task<List<UserOut>> Get()
    {
        using var connection = new NpgsqlConnection(dbSettings.ConnectionString);

        const string sql = @"
            SELECT
                u.id,
                u.name AS name,
                u.email,
                u.institution_id,
                i.name AS institution,
                STRING_AGG(r.name, ',') AS role
            FROM
                syki.users u
            INNER JOIN
                syki.institutions i ON i.id = u.institution_id
            INNER JOIN
                syki.user_roles ur ON ur.user_id = u.id
            INNER JOIN
                syki.roles r ON r.id = ur.role_id
            GROUP BY
                u.id, i.name
        ";

        var data = await connection.QueryAsync<UserOut>(sql);
        
        return data.ToList();
    }
}
