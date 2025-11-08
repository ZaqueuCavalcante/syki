using Exato.Back.Intelligence.Domain.Ibge;

namespace Exato.Tests.Integration.Seed;

public class IbgeDataSeeder(BackDbContext ctx)
{
    public async Task Run()
    {
        await SeedCnaeConsolidados();
    }

    private async Task SeedCnaeConsolidados()
    {
        var cnae = new CnaeConsolidado
        {
            Secao = "K",
            Divisao = "64",
            DivisaoNum = 64,
            Grupo = "64.3",
            GrupoNum = 643,
            Classe = "64.36-1",
            ClasseNum = 64361,
            Subclasse = "6436-1/00",
            SubclasseNum = 6436100,
            Denominacao = "Sociedades de crédito = financiamento e investimento - financeiras",
            Versao = 2.3M,
            ControleId = 6,
            Tipo = "SC",
            SegmentoQuod = 3,
        };

        await ctx.SaveChangesAsync(cnae);
    }
}
