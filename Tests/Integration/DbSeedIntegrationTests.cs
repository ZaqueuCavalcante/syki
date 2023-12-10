using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Integration;

[TestFixture]
public class DbSeedIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_fazer_o_seed_do_banco_de_dados()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        _ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        DbSeed.NovaRoma.Cursos[1].Disciplinas = DbSeed.NovaRoma.Disciplinas.Take(31).ToList();
        var disciplinasDireito = DbSeed.NovaRoma.Disciplinas.Skip(31).Take(39).ToList();
        disciplinasDireito.Add(DbSeed.NovaRoma.Disciplinas.First(x => x.Nome == "Informática e Sociedade"));
        DbSeed.NovaRoma.Cursos[4].Disciplinas = disciplinasDireito;

        // Act
        _ctx.Faculdades.Add(DbSeed.NovaRoma);
        _ctx.AddRange(DbSeed.Periodos);
        _ctx.SaveChanges();

        // Assert
        var faculdades = await _ctx.Faculdades.ToListAsync();
        faculdades.Should().HaveCount(2);
    }
}
