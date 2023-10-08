using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Database;
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
        _ctx.SeedStartupData();
        
        // Assert
        var faculdades = await _ctx.Faculdades.ToListAsync();
        var disciplinas = await _ctx.Disciplinas.ToListAsync();
        faculdades.Should().HaveCount(1);
    }
}
