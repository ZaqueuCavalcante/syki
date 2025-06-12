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

        var data = await connection.QueryFirstAsync<GetUserAccountOut>(sql, new { userId });

        if (data.Role == UserRole.Student)
        {
            const string studentSql = @"
                SELECT name FROM syki.courses WHERE id =
                (
                    SELECT course_id
                    FROM syki.course_offerings co
                    INNER JOIN syki.students s ON s.course_offering_id = co.id
                    WHERE s.id = @UserId
                )
            ";
            data.Course = await connection.QueryFirstAsync<string?>(studentSql, new { userId });
        }

        return data;
    }
}
