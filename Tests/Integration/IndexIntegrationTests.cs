using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class IndexIntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAdm))]
    public async Task Nao_deve_retornar_os_dados_de_index_quando_o_usuario_nao_eh_adm(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        // Act
        var response = await _client.GetAsync("/index/adm");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_os_dados_de_index_quando_o_usuario_nao_eh_academico(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        // Act
        var response = await _client.GetAsync("/index/academico");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAluno))]
    public async Task Nao_deve_retornar_os_dados_de_index_quando_o_usuario_nao_eh_aluno(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        // Act
        var response = await _client.GetAsync("/index/aluno");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Deve_retornar_os_dados_de_index_do_adm()
    {
        // Arrange
        await CreateFaculdade("USP");
        await CreateFaculdade("UFPE");
        var faculdade = await CreateFaculdade("Nova Roma");
        var userAcademico = UserIn.New(faculdade.Id, Academico);
        var userAluno = UserIn.New(faculdade.Id, Aluno);
        await RegisterUser(userAcademico);
        await RegisterUser(userAluno);

        // Act
        var response = await GetAsync<IndexAdmOut>("/index/adm");

        // Assert
        response.Faculdades.Should().Be(3);
        response.Users.Should().Be(2);
    }

    [Test]
    public async Task Deve_retornar_os_dados_de_index_do_academico()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        await PostAsync("/campi", new CampusIn { Nome = "Suassuna", Cidade = "Recife - PE" });
        await PostAsync("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });

        await PostAsync("/cursos", new CursoIn { Nome = "Direito", Tipo = TipoDeCurso.Tecnologo });
        await PostAsync("/cursos", new CursoIn { Nome = "Pedagogia", Tipo = TipoDeCurso.Mestrado });
        await PostAsync("/cursos", new CursoIn { Nome = "Ciência da Computação", Tipo = TipoDeCurso.Especializacao });
        await PostAsync("/cursos", new CursoIn { Nome = "Administração", Tipo = TipoDeCurso.Doutorado });
        await PostAsync("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = TipoDeCurso.Bacharelado });

        await PostAsync("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 });
        await PostAsync("/disciplinas", new DisciplinaIn { Nome = "Estrutura de Dados", CargaHoraria = 60 });
        await PostAsync("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", CargaHoraria = 55 });

        // Act
        var response = await GetAsync<IndexAcademicoOut>("/index/academico");

        // Assert
        response.Campus.Should().Be(2);
        response.Cursos.Should().Be(5);
        response.Disciplinas.Should().Be(3);
        response.Grades.Should().Be(0);
        response.Ofertas.Should().Be(0);
        response.Turmas.Should().Be(0);
        response.Professores.Should().Be(0);
        response.Alunos.Should().Be(0);
        response.Livros.Should().Be(0);
    }

    [Test]
    public async Task Deve_retornar_os_dados_de_index_do_aluno()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Aluno);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        var response = await GetAsync<IndexAlunoOut>("/index/aluno");

        // Assert
        response.DisciplinasConcluidas.Should().Be(5);
        response.DisciplinasTotal.Should().Be(78);
        response.Media.Should().Be(7.9M);
        response.CR.Should().Be(6.5M);
    }
}
