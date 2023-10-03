using Syki.Back.Database;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Integration;

[TestFixture]
public class DbSeedTests : ApiTestBase
{
    [Test]
    public async Task Deve_fazer_o_seed_do_banco_de_dados()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        _ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        // Act
        _ctx.Faculdades.Add(DbSeed.NovaRoma);
        await _ctx.SaveChangesAsync();
        
        // Assert
        var faculdades = await _ctx.Faculdades.ToListAsync();
        var campi = await _ctx.Campi.ToListAsync();
        var cursos = await _ctx.Cursos.ToListAsync();
        var disciplinas = await _ctx.Disciplinas.ToListAsync();
        faculdades.Should().HaveCount(1);
        campi.Should().HaveCount(3);
        cursos.Should().HaveCount(9);
        disciplinas.Should().HaveCount(70);
    }
}
