using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_periodo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        var body = new PeriodoIn { Id = "2023.1", Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) };

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
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        // Act
        await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.1", Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) });
        await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });

        // Assert
        var periodos = await client.GetAsync<List<string>>("/periodos");
        periodos.Should().HaveCount(2);
    }

    [Test]
    public async Task Deve_retornar_apenas_os_periodos_da_faculdade_do_usuario_logado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var novaRoma = await client.CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await client.RegisterUser(userNovaRoma);

        var ufpe = await client.CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await client.RegisterUser(userUfpe);

        await client.Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new PeriodoIn { Id = "2023.1", Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) };
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", bodyNovaRoma);

        await client.Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) };
        await client.PostAsync<PeriodoOut>("/periodos", bodyUfpe);

        // Act
        await client.Login(userNovaRoma.Email, userNovaRoma.Password);

        // Assert
        var periodos = await client.GetAsync<List<string>>("/periodos");
        periodos.Should().HaveCount(1);
        periodos[0].Should().Be(periodo.Id);
    }
}
