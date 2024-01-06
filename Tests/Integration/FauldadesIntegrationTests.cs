using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Integration;

[TestFixture]
public class FauldadesIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_faculdade()
    {
        // Arrange
        await Login("adm@syki.com", "Adm@123");

        var body = new FaculdadeIn { Nome = "Nova Roma" };

        // Act
        var faculdade = await PostAsync<FaculdadeOut>("/faculdades", body);
        
        // Assert
        faculdade.Nome.Should().Be(body.Nome);
    }

    [Test]
    public async Task Deve_criar_varias_faculdades()
    {
        // Arrange
        await Login("adm@syki.com", "Adm@123");

        // Act
        await PostAsync("/faculdades", new FaculdadeIn { Nome = "Nova Roma" });
        await PostAsync("/faculdades", new FaculdadeIn { Nome = "UFPE" });

        // Assert
        var faculdades = await GetAsync<List<FaculdadeOut>>("/faculdades");
        faculdades.Should().HaveCount(2);
    }
}
