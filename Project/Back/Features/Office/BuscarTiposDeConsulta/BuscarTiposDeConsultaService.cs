using Dapper;
using Microsoft.Extensions.Caching.Hybrid;
using Exato.Shared.Features.Office.BuscarTiposDeConsulta;

namespace Exato.Back.Features.Office.BuscarTiposDeConsulta;

public class BuscarTiposDeConsultaService(BackDbContext ctx, HybridCache cache) : IOfficeService
{
    public async Task<BuscarTiposDeConsultaOut> Get(BuscarTiposDeConsultaIn data)
    {
        var tipos = await cache.GetOrCreateAsync(
            key: "TiposDeConsulta",
            state: ctx,
            factory: async (stateCtx, ct) =>
            {
                await stateCtx.Database.OpenConnectionAsync(ct);
                var connection = ctx.Database.GetDbConnection();

                const string sql = @"
                    SELECT
                        consulta_tipo_id AS id,
                        nome,
                        visivel,
                        disponivel
                    FROM
                        public.consulta_tipo
                    ORDER BY
                        disponivel DESC, visivel DESC, nome
                ";

                return (await connection.QueryAsync<BuscarTiposDeConsultaItemOut>(sql)).ToList();
            }
        );

        var id = data.Nome.OnlyNumbers();
        var predicate = tipos.Where(x =>
            data.Nome.IsEmpty() ||
            (id.HasValue() && x.Id.ToString().Contains(id)) ||
            x.Nome.Contains(data.Nome, StringComparison.OrdinalIgnoreCase));

        var total = predicate.Count();
        var items = predicate.ToList();

        return new BuscarTiposDeConsultaOut()
        {
            Total = total,
            Items = items,
        };
    }
}
