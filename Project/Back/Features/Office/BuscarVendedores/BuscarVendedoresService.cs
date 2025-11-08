using Dapper;
using Exato.Shared.Features.Office.BuscarVendedores;

namespace Exato.Back.Features.Office.BuscarVendedores;

public class BuscarVendedoresService(BackDbContext ctx) : IOfficeService
{
    public async Task<BuscarVendedoresOut> Get()
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string sql = @"
            SELECT DISTINCT exato_sales_contact
            FROM public.cliente
            WHERE exato_sales_contact IS NOT NULL
            ORDER BY exato_sales_contact
        ";

        var vendedores = (await connection.QueryAsync<string>(sql)).ToList();

        return new BuscarVendedoresOut()
        {
            Items = vendedores,
        };
    }
}
