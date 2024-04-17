using Dapper;
using Npgsql;

namespace Syki.Back.GetUsers;

public class GetUsersService(DatabaseSettings dbSettings)
{
    public async Task<List<CreateUserOut>> Get()
    {
        using var connection = new NpgsqlConnection(dbSettings.ConnectionString);

        const string sql = @"
            SELECT
                u.id,
                u.name AS nome,
                u.email,
                i.nome AS institution,
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
                u.id, i.nome
        ";

        var data = await connection.QueryAsync<CreateUserOut>(sql);
        
        return data.ToList();
    }
}
