using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_fazer_o_seed_do_banco_de_dados()
    {
        // Arrange
        using var ctx = _factory.GetDbContext();

        DbSeed.NovaRoma.Cursos[1].Disciplinas = DbSeed.NovaRoma.Disciplinas.Take(31).ToList();
        var disciplinasDireito = DbSeed.NovaRoma.Disciplinas.Skip(31).Take(39).ToList();
        disciplinasDireito.Add(DbSeed.NovaRoma.Disciplinas.First(x => x.Nome == "Informática e Sociedade"));
        DbSeed.NovaRoma.Cursos[4].Disciplinas = disciplinasDireito;

        // Act
        ctx.Faculdades.Add(DbSeed.NovaRoma);
        ctx.AddRange(DbSeed.Periodos);
        ctx.SaveChanges();

        // Assert
        var faculdade = await ctx.Faculdades.FirstOrDefaultAsync(f => f.Id == DbSeed.NovaRoma.Id);
        faculdade.Should().NotBeNull();
    }
}
