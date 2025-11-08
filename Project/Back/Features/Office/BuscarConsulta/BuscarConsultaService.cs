using Dapper;
using System.Data.Common;
using Exato.Shared.Features.Office.BuscarConsulta;

namespace Exato.Back.Features.Office.BuscarConsulta;

public class BuscarConsultaService(BackDbContext ctx) : IOfficeService
{
    public async Task<BuscarConsultaOut> Get(string uid)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string sql = @"
            SELECT
                c.uid_base36 AS uid,
                c.consulta_tipo_id AS tipo_id,
                c.consulta_resultado_tipo_id AS resultado_id,
                c.token_acesso_id,
                ct.nome AS tipo,
                clt.nome AS cliente,
                c.inicio,
                c.fim,
                crt.nome AS resultado,
                c.pdf_resultado_compact_tam > 0 AS has_pdf,
                COALESCE(pdf_password, LEFT(LPAD(cpf_cnpj::text, 14, '0'), 6)) AS pdf_password
            FROM
                public.consulta c
            INNER JOIN
                public.consulta_tipo ct ON ct.consulta_tipo_id = c.consulta_tipo_id
            INNER JOIN
                public.token_acesso ta ON ta.token_acesso_id = c.token_acesso_id
            INNER JOIN
                public.cliente clt ON clt.cliente_id = ta.cliente_id
            INNER JOIN
                public.consulta_resultado_tipo crt ON crt.consulta_resultado_tipo_id = c.consulta_resultado_tipo_id
            WHERE
                c.uid_base36 = @Uid::citext
        ";

        var parameters = new { uid };
        var consulta = await connection.QueryFirstOrDefaultAsync<BuscarConsultaOut>(sql, parameters) ?? new();

        consulta.Subconsultas = await GetSubs(connection, [uid]);

        return consulta;
    }

    private static async Task<List<BuscarConsultaOut>> GetSubs(DbConnection connection, List<string> mastersIds)
    {
        var inString = mastersIds.ToSqlInList().Replace(" ", "");

        var subsSql = $@"
            SELECT
                c.uid_base36 AS uid,
                c.master_uid,
                c.consulta_tipo_id AS tipo_id,
                c.consulta_resultado_tipo_id AS resultado_id,
                c.token_acesso_id,
                ct.nome AS tipo,
                clt.nome AS cliente,
                c.inicio,
                c.fim,
                crt.nome AS resultado,
                c.pdf_resultado_compact_tam > 0 AS has_pdf,
                COALESCE(pdf_password, LEFT(LPAD(cpf_cnpj::text, 14, '0'), 6)) AS pdf_password
            FROM
                public.consulta c
            INNER JOIN
                public.consulta_tipo ct ON ct.consulta_tipo_id = c.consulta_tipo_id
            INNER JOIN
                public.token_acesso ta ON ta.token_acesso_id = c.token_acesso_id
            INNER JOIN
                public.cliente clt ON clt.cliente_id = ta.cliente_id
            INNER JOIN
                public.consulta_resultado_tipo crt ON crt.consulta_resultado_tipo_id = c.consulta_resultado_tipo_id
            WHERE
                c.master_uid IN {inString}
        ";

        return (await connection.QueryAsync<BuscarConsultaOut>(subsSql)).ToList() ?? [];
    }
}
