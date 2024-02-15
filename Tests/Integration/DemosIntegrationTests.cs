using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_o_registro_de_demo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var body = new DemoIn { Email = "demo00.facul@syki.com" };

        // Act
        var demo = await client.PostAsync<DemoOut>("/demos", body);

        // Assert
        demo.Email.Should().Be(body.Email);
    }

    [Test]
    public async Task Deve_criar_a_faculdade_e_o_usuario_academico_pra_demo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var demo = await client.PostAsync<DemoOut>("/demos", new DemoIn { Email = "demo01.facul@syki.com" });
        var token = await client.GetDemoSetupToken(demo.Email);

        // Act
        var setup = await client.PostAsync<DemoSetupOut>("/demos/setup", new DemoSetupIn { Token = token, Password = "Test@123" });

        // Assert
        setup.Ok.Should().BeTrue();
    }
}
