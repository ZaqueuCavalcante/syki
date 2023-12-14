using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class GradesIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_nova_grade()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var curso = await PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Análise e Desenvolvimento de Sistemas" });

        var bd = await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [curso.Id] });
        var ed = await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Estrutura de Dados", CargaHoraria = 60, Cursos = [curso.Id] });
        var poo = await PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Programação Orientada a Objetos", CargaHoraria = 55, Cursos = [curso.Id] });

        var body = new GradeIn {
            Nome = "Grade de ADS - 1.0",
            CursoId = curso.Id,
            Disciplinas = [
                new()
                {
                    Id = bd.Id,
                    Periodo = 1,
                    Creditos = 10,
                    CargaHoraria = 70,
                },
                new()
                {
                    Id = ed.Id,
                    Periodo = 2,
                    Creditos = 8,
                    CargaHoraria = 55,
                },
                new()
                {
                    Id = poo.Id,
                    Periodo = 3,
                    Creditos = 12,
                    CargaHoraria = 60,
                }
            ],
        };

        // Act
        var grade = await PostAsync<GradeOut>("/grades", body);

        // Assert
        grade.Id.Should().NotBeEmpty();
        grade.Nome.Should().Be(body.Nome);
        grade.CursoId.Should().Be(curso.Id);
        grade.CursoNome.Should().Be(curso.Nome);
        grade.Disciplinas.Should().HaveCount(3);        
    }

    [Test]
    public async Task Nao_deve_criar_uma_nova_grade_quando_o_usuario_nao_esta_logado()
    {
        // Arrange
        var body = new GradeIn { Nome = "Grade ADS - 1.0" };

        // Act
        var response = await _client.PostAsync("/grades", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_uma_nova_grade_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        await Login("adm@syki.com", "Adm@123");
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, role);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new GradeIn { Nome = "Grade ADS - 1.0" };

        // Act
        var response = await _client.PostAsync("/grades", body.ToStringContent());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_as_grades_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        await Login("adm@syki.com", "Adm@123");
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, role);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        // Act
        var response = await _client.GetAsync("/grades");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_retornar_as_diciplinas_da_grade_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        await Login("adm@syki.com", "Adm@123");
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, role);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var gradeId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/grades/{gradeId}/disciplinas");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
