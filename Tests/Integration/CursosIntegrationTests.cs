using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using static Syki.Shared.TipoDeCurso;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_curso()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        var body = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado };

        // Act
        var curso = await client.PostAsync<CursoOut>("/cursos", body);

        // Assert
        curso.Id.Should().NotBeEmpty();
        curso.Nome.Should().Be(body.Nome);
        curso.Tipo.Should().Be(body.Tipo);
    }

    [Test]
    public async Task Deve_criar_varios_cursos()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        // Act
        await client.PostAsync("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado });
        await client.PostAsync("/cursos", new CursoIn { Nome = "Direito", Tipo = Licenciatura });
        
        // Assert
        var cursos = await client.GetAsync<List<CursoOut>>("/cursos");
        cursos.Should().HaveCount(2);
    }

    [Test]
    public async Task Deve_retornar_apenas_os_cursos_da_faculdade_do_usuario_logado()
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
        var bodyNovaRoma = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado };
        await client.PostAsync<CursoOut>("/cursos", bodyNovaRoma);

        await client.Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new CursoIn { Nome = "Direito", Tipo = Licenciatura };
        await client.PostAsync<CursoOut>("/cursos", bodyUfpe);

        // Act
        await client.Login(userNovaRoma.Email, userNovaRoma.Password);

        var cursos = await client.GetAsync<List<CursoOut>>("/cursos");
        cursos.Should().HaveCount(1);

        // Assert
        cursos[0].Id.Should().NotBeEmpty();
        cursos[0].Nome.Should().Be(bodyNovaRoma.Nome);
        cursos[0].Tipo.Should().Be(bodyNovaRoma.Tipo);
    }

    [Test]
    public async Task Deve_retornar_os_cursos_ordenados_pelo_nome()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        // Act
        await client.PostAsync("/cursos", new CursoIn { Nome = "Direito", Tipo = Tecnologo });
        await client.PostAsync("/cursos", new CursoIn { Nome = "Pedagogia", Tipo = Mestrado });
        await client.PostAsync("/cursos", new CursoIn { Nome = "Ciência da Computação", Tipo = Especializacao });
        await client.PostAsync("/cursos", new CursoIn { Nome = "Administração", Tipo = Doutorado });
        await client.PostAsync("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado });

        // Assert
        var cursos = await client.GetAsync<List<CursoOut>>("/cursos");
        cursos.Should().HaveCount(5);
        cursos[0].Nome.Should().Be("Administração");
        cursos[1].Nome.Should().Be("Análise e Desenvolvimento de Sistemas");
        cursos[2].Nome.Should().Be("Ciência da Computação");
        cursos[3].Nome.Should().Be("Direito");
        cursos[4].Nome.Should().Be("Pedagogia");
    }
}
