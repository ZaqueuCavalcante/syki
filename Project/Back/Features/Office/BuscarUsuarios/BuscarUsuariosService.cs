using Dapper;
using Exato.Web;
using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.BuscarUsuarios;

namespace Exato.Back.Features.Office.BuscarUsuarios;

public class BuscarUsuariosService(BackDbContext ctx, WebDbContext webCtx) : IOfficeService
{
    public async Task<BuscarUsuariosOut> Get(BuscarUsuariosIn data)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string totalSql = @"
            SELECT
                count(1)
            FROM
                public.users
            WHERE
                (@IsActive IS NULL OR active = @IsActive)
                    AND
                (@Term IS NULL OR (cpf::text ILIKE @Term OR email ILIKE @Term OR full_name ILIKE @Term))
        ";

        const string itemsSql = @"
            SELECT
                id,
                external_id,
                full_name,
                email,
                cpf::text,
                created_at,
                active
            FROM
                public.users
            WHERE
                (@IsActive IS NULL OR active = @IsActive)
                    AND
                (@Term IS NULL OR (cpf::text ILIKE @Term OR email ILIKE @Term OR full_name ILIKE @Term))
            ORDER BY
                created_at DESC
            LIMIT 10
            OFFSET @Offset
        ";

        var parameters = new
        {
            data.IsActive,
            Offset = data.Page * 10,
            Term = data.SearchTerm.HasValue() ? $"%{data.SearchTerm.Trim()}%" : null,
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var usuarios = (await connection.QueryAsync<User>(itemsSql, parameters)).ToList();

        await webCtx.Database.OpenConnectionAsync();
        var webCnn = webCtx.Database.GetDbConnection();

        const string webSql = @"
            SELECT
                u.user_external_id::uuid AS external_id,
                MAX(l.event_date) AS last_access_at
            FROM
                public.user_companies u
            LEFT JOIN
                public.activities_logs l ON l.user_id = u.user_id
            WHERE
                u.user_external_id = ANY(@Ids)
            GROUP BY
                u.user_external_id
        ";
        var webParameters = new { Ids = usuarios.Where(x => x.ExternalId != null).Select(x => x.ExternalId.ToString()).ToArray() };
        var dates = (await webCnn.QueryAsync<WebUserLastAccessDto>(webSql, webParameters)).ToList();

        return new BuscarUsuariosOut()
        {
            Total = total,
            Items = usuarios.ConvertAll(x =>
                new BuscarUsuariosItemOut()
                {
                    Id = x.Id,
                    Nome = x.FullName ?? "-",
                    Email = x.Email ?? "-",
                    Documento = x.GetCpf() ?? "-",
                    CriadoEm = x.CreatedAt,
                    Ativo = x.Active,
                    LastAccessAt = dates.FirstOrDefault(d => d.ExternalId == x.ExternalId)?.LastAccessAt,
                }
            )
        };
    }
}

public class WebUserLastAccessDto
{
    public Guid ExternalId { get; set; }
    public DateTime? LastAccessAt { get; set; }
}
