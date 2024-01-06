using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using static Syki.Shared.TipoDeCurso;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class CursosIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_curso()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado };

        // Act
        var curso = await PostAsync<CursoOut>("/cursos", body);

        // Assert
        curso.Id.Should().NotBeEmpty();
        curso.Nome.Should().Be(body.Nome);
        curso.Tipo.Should().Be(body.Tipo);
    }

    [Test]
    public async Task Deve_criar_varios_cursos()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        await PostAsync("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado });
        await PostAsync("/cursos", new CursoIn { Nome = "Direito", Tipo = Licenciatura });
        
        // Assert
        var cursos = await GetAsync<List<CursoOut>>("/cursos");
        cursos.Should().HaveCount(2);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_um_novo_curso_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        var body = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado };

        // Act
        var response = await _client.PostAsync("/cursos", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_os_cursos_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        // Act
        var response = await _client.GetAsync("/cursos");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Deve_retornar_apenas_os_cursos_da_faculdade_do_usuario_logado()
    {
        // Arrange
        var novaRoma = await CreateFaculdade("Nova Roma");
        var userNovaRoma = UserIn.New(novaRoma.Id, Academico);
        await RegisterUser(userNovaRoma);

        var ufpe = await CreateFaculdade("UFPE");
        var userUfpe = UserIn.New(ufpe.Id, Academico);
        await RegisterUser(userUfpe);

        await Login(userNovaRoma.Email, userNovaRoma.Password);
        var bodyNovaRoma = new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado };
        await PostAsync<CursoOut>("/cursos", bodyNovaRoma);

        await Login(userUfpe.Email, userUfpe.Password);
        var bodyUfpe = new CursoIn { Nome = "Direito", Tipo = Licenciatura };
        await PostAsync<CursoOut>("/cursos", bodyUfpe);

        // Act
        await Login(userNovaRoma.Email, userNovaRoma.Password);

        var cursos = await GetAsync<List<CursoOut>>("/cursos");
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
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        await PostAsync("/cursos", new CursoIn { Nome = "Direito", Tipo = Tecnologo });
        await PostAsync("/cursos", new CursoIn { Nome = "Pedagogia", Tipo = Mestrado });
        await PostAsync("/cursos", new CursoIn { Nome = "Ciência da Computação", Tipo = Especializacao });
        await PostAsync("/cursos", new CursoIn { Nome = "Administração", Tipo = Doutorado });
        await PostAsync("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = Bacharelado });

        // Assert
        var cursos = await GetAsync<List<CursoOut>>("/cursos");
        cursos.Should().HaveCount(5);
        cursos[0].Nome.Should().Be("Administração");
        cursos[1].Nome.Should().Be("Análise e Desenvolvimento de Sistemas");
        cursos[2].Nome.Should().Be("Ciência da Computação");
        cursos[3].Nome.Should().Be("Direito");
        cursos[4].Nome.Should().Be("Pedagogia");
    }
}
