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
                f.nome AS faculdade,
                STRING_AGG(r.name, ',') AS role
            FROM
                syki.users u
            INNER JOIN
                syki.faculdades f ON f.id = u.institution_id
            INNER JOIN
                syki.user_roles ur ON ur.user_id = u.id
            INNER JOIN
                syki.roles r ON r.id = ur.role_id
            GROUP BY
                u.id, f.nome
        ";

        var data = await connection.QueryAsync<CreateUserOut>(sql);
        
        return data.ToList();
    }
}
