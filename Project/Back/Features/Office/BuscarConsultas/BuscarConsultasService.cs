using Dapper;
using System.Text;
using Exato.Shared.Features.Office.BuscarConsultas;

namespace Exato.Back.Features.Office.BuscarConsultas;

public class BuscarConsultasService(BackDbContext ctx) : IOfficeService
{
    public async Task<BuscarConsultasOut> Get(BuscarConsultasIn data)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        var builder = new StringBuilder();

        builder.Append(@"
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
                c.master_uid IS NULL AND c.consulta_master_id IS NULL
        ");

        if (data.Uid.IsEmpty()) builder.AppendLine(@"AND c.inicio > @Start AND c.inicio < @End");

        if (data.ClienteId > 0) builder.AppendLine(@"AND ta.cliente_id = @ClienteId");

        if (data.TipoId > 0) builder.AppendLine(@"AND c.consulta_tipo_id = @TipoId");

        if (data.ResultadoId > 0) builder.AppendLine(@"AND c.consulta_resultado_tipo_id = @ResultadoId");

        if (data.Uid.HasValue()) builder.AppendLine(@"AND c.uid_base36 = @Uid::citext");

        if (data.Chave.HasValue()) builder.AppendLine(@"AND c.chave = @Chave::citext");

        if (data.Document.HasValue()) builder.AppendLine(@"AND c.document = @Document::citext");

        if (data.Uid.IsEmpty()) builder.AppendLine(@"ORDER BY c.inicio DESC LIMIT 50");

        var parameters = new
        {
            data.End,
            data.Start,
            data.TipoId,
            data.ClienteId,
            data.ResultadoId,
            Uid = data.Uid.HasValue() ? data.Uid.Trim() : null,
            Chave = data.Chave.HasValue() ? data.Chave.Trim() : null,
            Document = data.Document.HasValue() ? data.Document.Trim() : null,
        };

        var items = (await connection.QueryAsync<BuscarConsultasItemOut>(builder.ToString(), parameters)).ToList();

        return new BuscarConsultasOut()
        {
            Total = items.Count,
            Items = items.Skip(data.Page * 10).Take(10).ToList(),
        };
    }
}
