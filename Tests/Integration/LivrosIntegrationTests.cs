using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class LivrosIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_livro()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new LivroIn { Titulo = "Manual de DevOps" };

        // Act
        var livro = await PostAsync<LivroOut>("/livros", body);

        // Assert
        livro.Id.Should().NotBeEmpty();
        livro.Titulo.Should().Be(body.Titulo);
    }

    [Test]
    public async Task Deve_criar_varios_livros_para_uma_mesma_faculdade()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        await PostAsync<LivroOut>("/livros", new LivroIn { Titulo = "Manual de DevOps" });
        await PostAsync<LivroOut>("/livros", new LivroIn { Titulo = "O Projeto do Projeto" });

        // Assert
        var livros = await GetAsync<List<LivroOut>>("/livros");
        livros.Should().HaveCount(2);
    }

    [Test]
    public async Task Nao_deve_criar_um_novo_livro_quando_o_usuario_esta_deslogado()
    {
        // Arrange
        var body = new LivroIn { Titulo = "Manual de DevOps" };

        // Act
        var response = await _client.PostAsync("/livros", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_um_novo_livro_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        var body = new LivroIn { Titulo = "Manual de DevOps" };

        // Act
        var response = await _client.PostAsync("/livros", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_os_livros_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        // Act
        var response = await _client.GetAsync("/livros");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Deve_retornar_apenas_os_livros_da_faculdade_do_usuario_logado()
    {
        // Arrange
        var novaRoma = await CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await RegisterUser(userNovaRoma);

        var ufpe = await CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await RegisterUser(userUfpe);

        await Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new LivroIn { Titulo = "Manual de DevOps" };
        var livro = await PostAsync<CampusOut>("/livros", bodyNovaRoma);

        await Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new LivroIn { Titulo = "O Projeto do Projeto" };
        await PostAsync<CampusOut>("/livros", bodyUfpe);

        // Act
        await Login(userNovaRoma.Email, userNovaRoma.Password);

        // Assert
        var campi = await GetAsync<List<LivroOut>>("/livros");
        campi.Should().HaveCount(1);
        campi[0].Id.Should().Be(livro.Id);
        campi[0].Titulo.Should().Be(bodyNovaRoma.Titulo);
    }
}
