using Dapper;
using Microsoft.Extensions.Caching.Hybrid;
using Exato.Shared.Features.Office.BuscarTiposDeResultado;

namespace Exato.Back.Features.Office.BuscarTiposDeResultado;

public class BuscarTiposDeResultadoService(BackDbContext ctx, HybridCache cache) : IOfficeService
{
    public async Task<BuscarTiposDeResultadoOut> Get(BuscarTiposDeResultadoIn data)
    {
        var tipos = await cache.GetOrCreateAsync(
            key: "TiposDeResultado",
            state: ctx,
            factory: async (stateCtx, ct) =>
            {
                await stateCtx.Database.OpenConnectionAsync(ct);
                var connection = ctx.Database.GetDbConnection();

                const string sql = @"
                    SELECT
                        consulta_resultado_tipo_id AS id,
                        nome
                    FROM
                        public.consulta_resultado_tipo
                    ORDER BY
                        nome
                ";

                return (await connection.QueryAsync<BuscarTiposDeResultadoItemOut>(sql)).ToList();
            }
        );

        var id = data.Nome.OnlyNumbers();
        var predicate = tipos.Where(x =>
            data.Nome.IsEmpty() ||
            (id.HasValue() && x.Id.ToString().Contains(id)) ||
            x.Nome.Contains(data.Nome, StringComparison.OrdinalIgnoreCase));

        var total = predicate.Count();
        var items = predicate.ToList();

        return new BuscarTiposDeResultadoOut()
        {
            Total = total,
            Items = items,
        };
    }
}
