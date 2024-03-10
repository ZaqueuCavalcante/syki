using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    // [Test]
    public async Task Deve_criar_uma_nova_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        await client.LoginAsAdm();

        var body = new FaculdadeIn { Nome = "Nova Roma" };

        // Act
        var faculdade = await client.PostAsync<FaculdadeOut>("/faculdades", body);
        
        // Assert
        faculdade.Nome.Should().Be(body.Nome);
    }

    // [Test]
    public async Task Deve_criar_varias_faculdades()
    {
        // Arrange
        var client = _factory.CreateClient();
        await client.LoginAsAdm();

        // Act
        await client.PostAsync("/faculdades", new FaculdadeIn { Nome = "Nova Roma" });
        await client.PostAsync("/faculdades", new FaculdadeIn { Nome = "UFPE" });

        // Assert
        var faculdades = await client.GetAsync<List<FaculdadeOut>>("/faculdades");
        faculdades.Should().HaveCountGreaterThanOrEqualTo(2);
    }
}
