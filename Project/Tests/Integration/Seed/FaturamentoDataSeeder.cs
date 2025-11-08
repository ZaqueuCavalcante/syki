using Exato.Back.Intelligence.Domain.Faturamento;

namespace Exato.Tests.Integration.Seed;

public class FaturamentoDataSeeder(BackDbContext ctx)
{
    public async Task Run()
    {
        await SeedPlanosRelatorios();
        await SeedPlanosCredito();
    }

    private async Task SeedPlanosRelatorios()
    {
        var planoA = new PlanosRelatorio
        {
            Id = 1, Nome = "Plano A", Padrao = true,
        };

        var planoB = new PlanosRelatorio
        {
            Id = 2, Nome = "Plano B",
        };

        ctx.AddRange(planoA, planoB);
        await ctx.SaveChangesAsync();
    }

    private async Task SeedPlanosCredito()
    {
        var planoA = new PlanosCredito
        {
            Id = 1, Nome = "Plano A", IsDefault = true,
        };

        var planoB = new PlanosCredito
        {
            Id = 2, Nome = "Plano B",
        };

        ctx.AddRange(planoA, planoB);
        await ctx.SaveChangesAsync();
    }
}
