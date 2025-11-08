using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.BuscarEmpresa;

namespace Exato.Back.Features.Office.BuscarEmpresa;

public class BuscarEmpresaService(BackDbContext ctx) : IOfficeService
{
    public async Task<OneOf<BuscarEmpresaOut, ExatoError>> Get(int id)
    {
        var empresa = await ctx.PublicCliente.AsNoTracking()
            .Where(x => x.ClienteId == id)
            .Select(x => new Cliente
            {
                ClienteId = x.ClienteId,
                Ativo = x.Ativo,
                CpfCnpj = x.CpfCnpj,
                Nome = x.Nome,
                RazaoSocialRf = x.RazaoSocialRf,
                ParentOrganizationId = x.ParentOrganizationId,
                NomeFantasiaRf = x.NomeFantasiaRf,
                QuodSegmentId = x.QuodSegmentId,
                Slug = x.Slug,
                ExatoSalesContact = x.ExatoSalesContact,

                HighPerformance = x.HighPerformance,
                BlockSensitiveDataInQueryString = x.BlockSensitiveDataInQueryString,
                DataAccessLevel = x.DataAccessLevel,
                TransLimitPerWeek = x.TransLimitPerWeek,

                GerarPdfConsultas = x.GerarPdfConsultas,
                HabilitarConsultasPorEmail = x.HabilitarConsultasPorEmail,

                ReceitaCpfUseSerproAsMainSource = x.ReceitaCpfUseSerproAsMainSource,
                ReceitaCpfShouldReturnMinor18AgeData = x.ReceitaCpfShouldReturnMinor18AgeData,

                DossierIdToExecutePf = x.DossierIdToExecutePf,
                DossierIdToExecutePfCreditAnalysis = x.DossierIdToExecutePfCreditAnalysis,
                DossierIdToExecutePj = x.DossierIdToExecutePj,
                DossierIdToExecutePjCreditAnalysis = x.DossierIdToExecutePjCreditAnalysis,

                IsBillingCustomer = x.IsBillingCustomer,
                Saldo = x.Saldo,
                BalanceInBrl = x.BalanceInBrl,
                FaturamentoTipoId = x.FaturamentoTipoId,
                BalanceType = x.BalanceType,

                IncluidoEm = x.IncluidoEm,
                ExternalId = x.ExternalId,
            })
            .FirstOrDefaultAsync();

        if (empresa == null) return EmpresaNaoEncontrada.I;

        empresa.IsParent = await ctx.PublicCliente.AnyAsync(x => x.ParentOrganizationId == id);

        var matriz = await ctx.PublicCliente.AsNoTracking()
            .Where(x => x.ClienteId == empresa.ParentOrganizationId)
            .Select(x => new { x.ClienteId, x.Nome })
            .FirstOrDefaultAsync();

        var possuiFiliais = await ctx.PublicCliente
            .AnyAsync(x => x.ParentOrganizationId == empresa.ClienteId);

        var relatoriosIds = empresa.Relatorios();
        var relatorios = await ctx.PublicConsultaTipo.AsNoTracking()
            .Where(x => relatoriosIds.Contains(x.ConsultaTipoId))
            .Select(x => new BuscarEmpresaRelatorioOut { Id = x.ConsultaTipoId, Nome = x.Nome })
            .ToListAsync() ?? [];

        return empresa.ToBuscarEmpresaOut(matriz?.ClienteId, matriz?.Nome, possuiFiliais, relatorios);
    }
}
