using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.BuscarEmpresa;

namespace Exato.Back.Features.Office.BuscarEmpresa;

public static class BuscarEmpresaMapper
{
    extension(Cliente empresa)
    {
        public BuscarEmpresaOut ToBuscarEmpresaOut(int? matrizId, string? matriz, bool possuiFiliais, List<BuscarEmpresaRelatorioOut> relatorios)
        {
            return new BuscarEmpresaOut()
            {
                Id = empresa.ClienteId,
                Ativa = empresa.Ativo,
                Tipo = empresa.GetTipo(),
                CNPJ = empresa.GetDocument() ?? "-",
                Nome = empresa.Nome,
                RazaoSocial = empresa.RazaoSocialRf ?? "-",
                MatrizId = matrizId,
                Matriz = matriz ?? "-",
                NomeFantasia = empresa.NomeFantasiaRf,
                QuodSegmentId = empresa.QuodSegmentId,
                Slug = empresa.Slug,
                SalesContact = empresa.ExatoSalesContact,

                HighPerformance = empresa.HighPerformance,
                BlockSensitiveDataInQueryString = empresa.BlockSensitiveDataInQueryString ?? false,
                DataAccessLevel = empresa.DataAccessLevel.ToEnum<DataAccessLevel>(),
                TransLimitPerWeek = empresa.TransLimitPerWeek,

                GerarPdfConsultas = empresa.GerarPdfConsultas,
                HabilitarConsultasPorEmail = empresa.HabilitarConsultasPorEmail,

                ReceitaCpfUseSerproAsMainSource = empresa.ReceitaCpfUseSerproAsMainSource,
                ReceitaCpfShouldReturnMinor18AgeData = empresa.ReceitaCpfShouldReturnMinor18AgeData,

                RelatorioPF = relatorios.FirstOrDefault(x => x.Id == empresa.DossierIdToExecutePf) ?? new(),
                RelatorioPFQuod = relatorios.FirstOrDefault(x => x.Id == empresa.DossierIdToExecutePfCreditAnalysis) ?? new(),
                RelatorioPJ = relatorios.FirstOrDefault(x => x.Id == empresa.DossierIdToExecutePj) ?? new(),
                RelatorioPJQuod = relatorios.FirstOrDefault(x => x.Id == empresa.DossierIdToExecutePjCreditAnalysis) ?? new(),

                IsBillingCustomer = empresa.IsBillingCustomer ?? false,
                Creditos = empresa.Saldo,
                BalanceInBrl = empresa.BalanceInBrl ?? 0,
                MetodoDePagamento = (empresa.FaturamentoTipoId ?? 1).ToEnum<MetodoDePagamento>(),
                BalanceType = empresa.BalanceType.ToEnum<BalanceType>(),

                CriadaEm = empresa.IncluidoEm,
                ExternalId = empresa.ExternalId,
                PossuiFiliais = possuiFiliais,
            };
        }
    }
}
