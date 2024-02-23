using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using static Syki.Back.Configs.AuthorizationConfigs;
using Syki.Shared.CreateUser;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_periodo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new PeriodoIn("2023.2");

        // Act
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", body);

        // Assert
        periodo.Id.Should().Be(periodo.Id);
    }

    [Test]
    public async Task Deve_criar_varios_periodos_para_uma_mesma_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        // Act
        await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2023.1"));
        await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2023.2"));

        // Assert
        var periodos = await client.GetAsync<List<PeriodoOut>>("/periodos");
        periodos.Should().HaveCount(2);
    }

    [Test]
    public async Task Deve_retornar_apenas_os_periodos_da_faculdade_do_usuario_logado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var novaRoma = await client.CreateInstitution("Nova Roma");
        var userNovaRoma = CreateUserIn.New(novaRoma.Id, Academico);
        await client.RegisterUser(userNovaRoma);

        var ufpe = await client.CreateInstitution("UFPE");
        var userUfpe = CreateUserIn.New(ufpe.Id, Academico);
        await client.RegisterUser(userUfpe);

        await client.Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new PeriodoIn("2023.1");
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", bodyNovaRoma);

        await client.Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new PeriodoIn("2023.2");
        await client.PostAsync<PeriodoOut>("/periodos", bodyUfpe);

        // Act
        await client.Login(userNovaRoma.Email, userNovaRoma.Password);

        // Assert
        var periodos = await client.GetAsync<List<PeriodoOut>>("/periodos");
        periodos.Should().HaveCount(1);
        periodos[0].Id.Should().Be(periodo.Id);
    }
}
