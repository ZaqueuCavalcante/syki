using Exato.Back.Intelligence.Domain.Public;

namespace Exato.Tests.Integration.Seed;

public class PublicDataSeeder(BackDbContext ctx)
{
    public async Task Run()
    {
        await SeedFaturamentoTipos();
        await SeedRealms();
        await SeedConsultaRelatorioTipos();
        await SeedConsultaTipos();
    }

    private async Task SeedFaturamentoTipos()
    {
        var prePago = new FaturamentoTipo { FaturamentoTipoId = 1, Nome = "Pré Pago" };
        var posPago = new FaturamentoTipo { FaturamentoTipoId = 2, Nome = "Pós Pago" };

        ctx.AddRange(prePago, posPago);

        await ctx.SaveChangesAsync();
    }

    private async Task SeedRealms()
    {
        var realm1 = new Realm { Id = 1, Name = "Exato", Description = "Exato Main Realm", CreatedAt = DateTime.Now };
        var realm2 = new Realm { Id = 2, Name = "B3 KYC Dossie", Description = "B3 KYC Dossie Realm", CreatedAt = DateTime.Now };
        var realm3 = new Realm { Id = 3, Name = "Checador", Description = "Checador Realm", CreatedAt = DateTime.Now };

        ctx.AddRange(realm1, realm2, realm3);

        await ctx.SaveChangesAsync();
    }

    private async Task SeedConsultaRelatorioTipos()
    {
        var pf = new ConsultaRelatorioTipo { Id = 1, Tipo = "PF" };
        var pj = new ConsultaRelatorioTipo { Id = 2, Tipo = "PJ" };
        var pfQuod = new ConsultaRelatorioTipo { Id = 3, Tipo = "PF + Quod" };
        var pjQuod = new ConsultaRelatorioTipo { Id = 4, Tipo = "PJ + Quod" };

        ctx.AddRange(pf, pfQuod, pj, pjQuod);

        await ctx.SaveChangesAsync();
    }

    private async Task SeedConsultaTipos()
    {
        var pf = new ConsultaTipo
        {
            ConsultaTipoId = 5030,
            Nome = "Exato - Relatório Pessoa Física",
            IsDossier = true,
            Disponivel = true,
            ConsultaRelatorioTipoId = 1,
        };
        var pfV2 = new ConsultaTipo
        {
            ConsultaTipoId = 5040,
            Nome = "Exato - Dossiê Pessoa Física - Full - V2",
            IsDossier = true,
            Disponivel = true,
            ConsultaRelatorioTipoId = 1,
        };
        var pfCriminal = new ConsultaTipo
        {
            ConsultaTipoId = 5033,
            Nome = "Background Check Criminal - Pessoa Física",
            IsDossier = true,
            Disponivel = true,
            ConsultaRelatorioTipoId = 1,
        };

        var pfQuod = new ConsultaTipo
        {
            ConsultaTipoId = 5031,
            Nome = "Exato - Relatório de Pessoa Física com Análise de Crédito",
            IsDossier = true,
            Disponivel = true,
            ConsultaRelatorioTipoId = 3,
        };
        var pfQuodV2 = new ConsultaTipo
        {
            ConsultaTipoId = 5041,
            Nome = "Exato - Dossiê Pessoa Física com Análise de Crédito - V2",
            IsDossier = true,
            Disponivel = true,
            ConsultaRelatorioTipoId = 3,
        };
        var pfQuodCriminal = new ConsultaTipo
        {
            ConsultaTipoId = 5034,
            Nome = "Background Check Criminal com Análise de Crédito - Pessoa Física",
            IsDossier = true,
            Disponivel = true,
            ConsultaRelatorioTipoId = 3,
        };

        var pj = new ConsultaTipo
        {
            ConsultaTipoId = 5025,
            Nome = "Exato - Relatório de Pessoa Jurídica",
            IsDossier = true,
            Disponivel = true,
            ConsultaRelatorioTipoId = 2,
        };
        var pjV2 = new ConsultaTipo
        {
            ConsultaTipoId = 5045,
            Nome = "Exato - Dossiê Empresa - Full - V2",
            IsDossier = true,
            Disponivel = true,
            ConsultaRelatorioTipoId = 2,
        };

        var pjQuod = new ConsultaTipo
        {
            ConsultaTipoId = 5026,
            Nome = "Exato - Relatório de Pessoa Jurídica com Análise de Crédito",
            IsDossier = true,
            Disponivel = true,
            ConsultaRelatorioTipoId = 4,
        };
        var pjQuodV2 = new ConsultaTipo
        {
            ConsultaTipoId = 5046,
            Nome = "Exato - Relatório de Pessoa Jurídica com Análise de Crédito - V2",
            IsDossier = true,
            Disponivel = true,
            ConsultaRelatorioTipoId = 4,
        };

        ctx.AddRange(pf, pfV2, pfCriminal, pfQuod, pfQuodV2, pfQuodCriminal, pj, pjV2, pjQuod, pjQuodV2);

        await ctx.SaveChangesAsync();
    }
}
