using Exato.Back.Intelligence.Domain.Public;
using Exato.Back.Intelligence.Domain.Faturamento;
using Exato.Shared.Features.Office.BuscarConfiguracoesDeFaturamentoDaEmpresa;

namespace Exato.Back.Features.Office.BuscarConfiguracoesDeFaturamentoDaEmpresa;

public class BuscarConfiguracoesDeFaturamentoDaEmpresaService(BackDbContext ctx) : IOfficeService
{
    public async Task<BuscarConfiguracoesDeFaturamentoDaEmpresaOut> Get(int id)
    {
        var empresa = await ctx.PublicCliente.AsNoTracking()
            .Where(x => x.ClienteId == id)
            .Select(x => new Cliente
            {
                IsBillingCustomer = x.IsBillingCustomer,
                Saldo = x.Saldo,
                BalanceInBrl = x.BalanceInBrl,
                FaturamentoTipoId = x.FaturamentoTipoId,
                BalanceType = x.BalanceType,
            })
            .FirstOrDefaultAsync();

        if (empresa == null) return new();

        var configs = await ctx.FaturamentoClienteConfig
            .Where(x => x.ClienteId == id)
            .FirstOrDefaultAsync();

        if (configs == null) return new ClienteConfig().ToOut(empresa);

        return configs.ToOut(empresa);
    }
}
