using Syki.Shared;
using Syki.Shared.CreateUser;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_retornar_os_dados_de_index_do_adm()
    {
        // Arrange
        var client = _factory.CreateClient();

        await client.CreateInstitution("USP");
        await client.CreateInstitution("UFPE");
        var faculdade = await client.CreateInstitution("Nova Roma");
        var userAcademico = CreateUserIn.New(faculdade.Id, Academico);
        var userAluno = CreateUserIn.New(faculdade.Id, Aluno);
        await client.RegisterUser(userAcademico);
        await client.RegisterUser(userAluno);

        // Act
        var response = await client.GetAsync<IndexAdmOut>("/index/adm");

        // Assert
        response.Faculdades.Should().BeGreaterThanOrEqualTo(3);
        response.Users.Should().BeGreaterThanOrEqualTo(2);
    }

    [Test]
    public async Task Deve_retornar_os_dados_de_index_do_academico()
    {
        // Arrange
        var client = _factory.GetClient();
        var faculdade = await client.CreateInstitution();
        await client.RegisterAndLogin(faculdade.Id, Academico);

        await client.NewCampus("Agreste I", "Caruaru - PE");
        await client.NewCampus("Suassuna I", "Recife - PE");

        await client.PostAsync("/cursos", new CursoIn { Nome = "Direito", Tipo = TipoDeCurso.Tecnologo });
        await client.PostAsync("/cursos", new CursoIn { Nome = "Pedagogia", Tipo = TipoDeCurso.Mestrado });
        await client.PostAsync("/cursos", new CursoIn { Nome = "Ciência da Computação", Tipo = TipoDeCurso.Especializacao });
        await client.PostAsync("/cursos", new CursoIn { Nome = "Administração", Tipo = TipoDeCurso.Doutorado });
        await client.PostAsync("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas", Tipo = TipoDeCurso.Bacharelado });

        await client.PostAsync("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados" });
        await client.PostAsync("/disciplinas", new DisciplinaIn { Nome = "Estrutura de Dados" });
        await client.PostAsync("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos" });

        // Act
        var response = await client.GetAsync<IndexAcademicoOut>("/index/academico");

        // Assert
        response.Campus.Should().Be(2);
        response.Cursos.Should().Be(5);
        response.Disciplinas.Should().Be(3);
        response.Grades.Should().Be(0);
        response.Ofertas.Should().Be(0);
        response.Turmas.Should().Be(0);
        response.Professores.Should().Be(0);
        response.Alunos.Should().Be(0);
        response.Notifications.Should().Be(0);
    }

    [Test]
    public async Task Deve_retornar_os_dados_de_index_do_aluno()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Aluno);

        // Act
        var response = await client.GetAsync<IndexAlunoOut>("/index/aluno");

        // Assert
        response.DisciplinasConcluidas.Should().Be(5);
        response.DisciplinasTotal.Should().Be(78);
        response.Media.Should().Be(7.9M);
        response.CR.Should().Be(6.5M);
    }
}
