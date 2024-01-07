using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_livro()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        var body = new LivroIn { Titulo = "Manual de DevOps" };

        // Act
        var livro = await client.PostAsync<LivroOut>("/livros", body);

        // Assert
        livro.Id.Should().NotBeEmpty();
        livro.Titulo.Should().Be(body.Titulo);
    }

    [Test]
    public async Task Deve_criar_varios_livros_para_uma_mesma_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        // Act
        await client.PostAsync<LivroOut>("/livros", new LivroIn { Titulo = "Manual de DevOps" });
        await client.PostAsync<LivroOut>("/livros", new LivroIn { Titulo = "O Projeto do Projeto" });

        // Assert
        var livros = await client.GetAsync<List<LivroOut>>("/livros");
        livros.Should().HaveCount(2);
    }

    [Test]
    public async Task Deve_retornar_apenas_os_livros_da_faculdade_do_usuario_logado()
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
        var bodyNovaRoma = new LivroIn { Titulo = "Manual de DevOps" };
        var livro = await client.PostAsync<CampusOut>("/livros", bodyNovaRoma);

        await client.Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new LivroIn { Titulo = "O Projeto do Projeto" };
        await client.PostAsync<CampusOut>("/livros", bodyUfpe);

        // Act
        await client.Login(userNovaRoma.Email, userNovaRoma.Password);

        // Assert
        var campi = await client.GetAsync<List<LivroOut>>("/livros");
        campi.Should().HaveCount(1);
        campi[0].Id.Should().Be(livro.Id);
        campi[0].Titulo.Should().Be(bodyNovaRoma.Titulo);
    }
}
