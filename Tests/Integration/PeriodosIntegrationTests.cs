using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class PeriodosIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_periodo()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new PeriodoIn { Id = "2023.1", Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) };

        // Act
        var periodo = await PostAsync<PeriodoOut>("/periodos", body);

        // Assert
        periodo.Id.Should().Be(periodo.Id);
    }

    [Test]
    public async Task Deve_criar_varios_periodos_para_uma_mesma_faculdade()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        await PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.1", Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) });
        await PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });

        // Assert
        var periodos = await GetAsync<List<string>>("/periodos");
        periodos.Should().HaveCount(2);
    }

    [Test]
    public async Task Nao_deve_criar_um_novo_periodo_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new PeriodoIn { Id = "2023.1", Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) };

        // Act
        var response = await _client.PostAsync("/periodos", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_um_novo_periodo_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        var body = new PeriodoIn { Id = "2023.1", Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) };

        // Act
        var response = await _client.PostAsync("/periodos", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_os_periodos_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        // Act
        var response = await _client.GetAsync("/periodos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Deve_retornar_apenas_os_periodos_da_faculdade_do_usuario_logado()
    {
        // Arrange
        var novaRoma = await CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await RegisterUser(userNovaRoma);

        var ufpe = await CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await RegisterUser(userUfpe);

        await Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new PeriodoIn { Id = "2023.1", Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) };
        var periodo = await PostAsync<PeriodoOut>("/periodos", bodyNovaRoma);

        await Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) };
        await PostAsync<PeriodoOut>("/periodos", bodyUfpe);

        // Act
        await Login(userNovaRoma.Email, userNovaRoma.Password);

        // Assert
        var periodos = await GetAsync<List<string>>("/periodos");
        periodos.Should().HaveCount(1);
        periodos[0].Should().Be(periodo.Id);
    }
}
