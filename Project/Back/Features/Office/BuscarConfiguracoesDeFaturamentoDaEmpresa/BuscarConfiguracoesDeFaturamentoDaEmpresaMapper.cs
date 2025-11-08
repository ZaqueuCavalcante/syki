using Exato.Back.Intelligence.Domain.Public;
using Exato.Back.Intelligence.Domain.Faturamento;
using Exato.Shared.Features.Office.BuscarConfiguracoesDeFaturamentoDaEmpresa;

namespace Exato.Back.Features.Office.BuscarConfiguracoesDeFaturamentoDaEmpresa;

public static class BuscarConfiguracoesDeFaturamentoDaEmpresaMapper
{
    extension(ClienteConfig config)
    {
        public BuscarConfiguracoesDeFaturamentoDaEmpresaOut ToOut(Cliente empresa)
        {
            return new()
            {
                IsBillingCustomer = empresa.IsBillingCustomer ?? false,
                Creditos = empresa.Saldo,
                BalanceInBrl = empresa.BalanceInBrl ?? 0,
                MetodoDePagamento = (empresa.FaturamentoTipoId ?? 1).ToEnum<MetodoDePagamento>(),
                BalanceType = empresa.BalanceType.ToEnum<BalanceType>(),
                FranquiaMinima = config.FranquiaMinima,
                UnmaskedCustomer = config.UnmaskedCustomer,
                SummaryCustomer = config.SummaryCustomer,
                PreviousCustomer = config.PreviousCustomer,
                FaturamentoPorFaixa = config.FaturamentoPorFaixa,
                V1Customer = config.V1Customer,
                FaturamentoPorRateio = config.FaturamentoPorRateio,
                DetalharRelatorios = config.DetalharRelatorios ?? false,
                ExibirNaoConsumidores = config.ExibirNaoConsumidores ?? false,
            };
        }
    }
}
