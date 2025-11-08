using Dapper;
using Exato.Shared.Features.Office.BuscarTiposDeRelatorios;

namespace Exato.Back.Features.Office.BuscarTiposDeRelatorios;

public class BuscarTiposDeRelatoriosService(BackDbContext ctx) : IOfficeService
{
    public async Task<BuscarTiposDeRelatoriosOut> Get()
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string sql = @"
            SELECT
                nome,
                consulta_tipo_id AS id,
                consulta_relatorio_tipo_id AS tipo
            FROM
                public.consulta_tipo
            WHERE
                is_dossier
                    AND
                disponivel
                    AND
                consulta_relatorio_tipo_id IN (1, 2, 3, 4)
                    AND
                consulta_tipo_id IN (5040, 5041, 5045, 5046, 5033, 5034)
        ";

        var tipos = (await connection.QueryAsync<TiposDeRelatoriosDto>(sql)).ToList();

        return new BuscarTiposDeRelatoriosOut
        {
            Pf = tipos.Where(x => x.Tipo == 1).Select(x => x.ToItemOut()).ToList(),
            Pj = tipos.Where(x => x.Tipo == 2).Select(x => x.ToItemOut()).ToList(),
            PfQuod = tipos.Where(x => x.Tipo == 3).Select(x => x.ToItemOut()).ToList(),
            PjQuod = tipos.Where(x => x.Tipo == 4).Select(x => x.ToItemOut()).ToList(),
        };
    }
}

public class TiposDeRelatoriosDto
{
    public short Id { get; set; }
    public int Tipo { get; set; }
    public string Nome { get; set; }

    public BuscarTiposDeRelatoriosItemOut ToItemOut()
    {
        return new BuscarTiposDeRelatoriosItemOut { Id = Id, Nome = Nome };
    }
}
