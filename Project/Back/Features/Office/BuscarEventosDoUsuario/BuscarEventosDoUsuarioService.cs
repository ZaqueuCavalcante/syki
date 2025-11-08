using Dapper;
using Exato.Web;
using Exato.Shared.Features.Office.BuscarEventosDoUsuario;

namespace Exato.Back.Features.Office.BuscarEventosDoUsuario;

public class BuscarEventosDoUsuarioService(WebDbContext webCtx) : IOfficeService
{
    public async Task<BuscarEventosDoUsuarioOut> Get(int id, BuscarEventosDoUsuarioIn data)
    {
        await webCtx.Database.OpenConnectionAsync();
        var connection = webCtx.Database.GetDbConnection();

        const string totalSql = @"
            SELECT
                count(1)
            FROM
                public.activities_logs
            WHERE
                user_id = @Id
        ";

        const string itemsSql = @"
            SELECT
                event_date,
                description
            FROM
                public.activities_logs
            WHERE
                user_id = @Id
            ORDER BY
                event_date DESC
            LIMIT 10
            OFFSET @Offset
        ";

        var parameters = new
        {
            id,
            Offset = data.Page * 10,
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var items = (await connection.QueryAsync<BuscarEventosDoUsuarioItemOut>(itemsSql, parameters)).ToList();

        return new BuscarEventosDoUsuarioOut()
        {
            Total = total,
            Items = items,
        };
    }
}
