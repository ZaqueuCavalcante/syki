using Dapper;

namespace Estud.Back.Features.Users.GetUserAccount;

public class GetUserAccountService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetUserAccountOut> Get()
    {
        await using var connection = await ctx.GetOpenConnectionAsync();

        const string sql = @"
            SELECT
                u.id,
                u.name,
                u.email,
                u.profile_photo,
                r.name AS role,
                r.base_type AS user_type,
                r.permissions,
                i.name AS institution
            FROM
                estud.users u
            INNER JOIN
                estud.user_roles ur ON ur.user_id = u.id
            INNER JOIN
                estud.roles r ON r.id = ur.role_id
            INNER JOIN
                estud.institutions i ON i.id = u.institution_id
            WHERE
                u.id = @UserId
            LIMIT 1
        ";

        return await connection.QueryFirstAsync<GetUserAccountOut>(sql, new { UserId = ctx.RequestUser.Id });
    }
}
